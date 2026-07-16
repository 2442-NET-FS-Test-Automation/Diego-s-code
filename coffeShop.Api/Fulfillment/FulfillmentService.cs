
using System.Collections.Concurrent;
using System.IO.Compression;
using coffeShop.Api.Exceptions;
using coffeShop.data;
using coffeShop.data.Entities;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace coffeShop.Api.Fulfillment;

//Just setting the interface
public interface IFulfillmentService
{

        public Task<FulfillmentResult> FulfillOneAsync(int orderId, CancellationToken ct);
        public Task<BurstResult> FulfillBurstAsync(IEnumerable<int> orderIds, CancellationToken ct);
        public int ResolveProductId(string sku);
        public Task LoadInventoryCacheAsync(CancellationToken ct);


    
}

public enum FulfillmentResult { Completed, Rejected }

public record BurstResult(int completed, int rejected);

//Call the methods
public class FulfillmentService : IFulfillmentService
{
    private readonly IDbContextFactory<coffeShopContext> _factory;
    private readonly BurstPlaner _planner;
    private readonly ConcurrentDictionary<string, int> _skuToproductId;
    private readonly ConcurrentDictionary<int, int> _inventoryCache = new();


//The constructors
    public FulfillmentService(IDbContextFactory<coffeShopContext> factory, BurstPlaner planer)
    {
        _factory = factory;
        _planner = planer;

        using var db = _factory.CreateDbContext();
        _skuToproductId = new ConcurrentDictionary<string, int>(
            db.Products.ToDictionary(p => p.Sku, p => p.ProductId)
        );
    }

    public async Task<FulfillmentResult> FulfillOneAsync(int orderId, CancellationToken ct)
    {
        
    try{ 

        ct.ThrowIfCancellationRequested();
        
        await using var db = await _factory.CreateDbContextAsync(ct);
        var order = await db.Orders.Include(o => o.Lines).FirstAsync(o => o.OrderId == orderId, ct);
        var requested = order.Lines.ToDictionary(l => l.ProductId, l => l.Quantity);

        bool canFulfill = true;

    
        //this is used to see if we have stock to sell our products
    foreach(OrderLine line in order.Lines)
    {
        bool lineFulfilled = false;
        while(true)
        {
        if(_inventoryCache.TryGetValue(line.ProductId, out int currentInventory)) 
        {        
            if(currentInventory < line.Quantity)
            {
                canFulfill = false; //If it isn't enough the order can't be fulfilled
                break;
            }

            if(_inventoryCache.TryUpdate(line.ProductId, currentInventory - line.Quantity, currentInventory)) //I try to update the stock in memory
                        {
                            lineFulfilled = true; //if it can be done, get out of the while!
                            break;
                        }
                        //If TryUpdate returns false, we try again the loop!
                    }
                    else
                    {
                        canFulfill = false; //if the item isn't founded break
                        break;
                    }    
        }

         if(!lineFulfilled) //If it is false break because it cannot be Fulfilled 
        break;
    }

        if (!canFulfill) //If can't fulfill the order reject it
        {
            order.Status = Status.Rejected.ToString(); //Set the status to rejected
            db.FulFillmentEvents.Add(new FulFillmentEvent {
                OrderId = orderId, 
                Type = "Rejected", 
                Message = "Not enough stock"}); //And update the order with the message

            await db.SaveChangesAsync(); //Save the changes
            Log.Warning("Rejected {orderId}: not enough stock", orderId); //Throw a warning 

            return FulfillmentResult.Rejected;
        }

        order.Status = Status.Completed.ToString(); //If we have the stock only set the status to complete
        order.CompletedAt = DateTime.UtcNow; //Update the time when the order was completed
        db.FulFillmentEvents.Add(new FulFillmentEvent { OrderId = orderId, Type = "Completed", Message = "Order Complete!"}); 

        foreach(var (productId, quantity) in requested)
            {
                var invEntity = await db.Inventory.FirstAsync(i => i.ProductId == productId, ct);
                if(invEntity.Quantity < quantity)
                        throw new NotEnoughStockException(productId, quantity, invEntity.Quantity);
                invEntity.Quantity -= quantity;
            }

        if(!await SaveWithRetryAsync(db, requested, ct)) //If the order can't be updated
        {
            db.ChangeTracker.Clear();
            Order staleOrder = await db.Orders.FirstAsync(o => o.OrderId == orderId, ct);
            staleOrder.Status = Status.Rejected.ToString(); //update the status to rejected
            Log.Warning("Rejected order {OrderId} after concurrency retry", orderId);
            return FulfillmentResult.Rejected; //Reject it
        }

        Log.Information("Fulfilled order: {OrderId}, {LineCount} lines", orderId, order.Lines.Count);
        return FulfillmentResult.Completed; //If everything goes right, order is completed
        }
        catch(Exception ex)
        {
            Log.Error(ex, "The order failed!");
            return FulfillmentResult.Rejected;
        }


    }
    
    //Here I use the custom exception
    public int ResolveProductId(string sku)
    {
        try
        {
            return _skuToproductId[sku];
        }
        catch(KeyNotFoundException)
        {
            throw new UnknowSkuException(sku);
        }

    }

    //Here we try to save the changes to de db
    private static async Task<bool> SaveWithRetryAsync(
        coffeShopContext db, IReadOnlyDictionary<int, int> requestedByProductId, CancellationToken ct)
    {
        while (true)
        {
            try
            {
                await db.SaveChangesAsync(ct); 
                return true;
            }
            catch (DbUpdateConcurrencyException ex) 
            {
                Log.Warning("Attempt retry");

                foreach(var entry in ex.Entries)
                {
                    var current = await entry.GetDatabaseValuesAsync();

                    if(current is null) return false; //If the product does not exist abort

                    entry.OriginalValues.SetValues(current);

                    if(entry.Entity is InventoryItem inv) //verify the collision happened in the InventoryItem
                    {
                        int newValue = current.GetValue<int> (nameof(InventoryItem.Quantity)); //Take the stock in inventory
                        int desiredAmount = requestedByProductId[inv.ProductId]; //The stock that we need
                        if(newValue < desiredAmount) return false; //If the real update stock is not enough abort!
                        inv.Quantity = newValue - desiredAmount; //If there is enough sustract the used stock
                    }
                }
            }
        }
    }

    public async Task<BurstResult> FulfillBurstAsync(IEnumerable<int> orderIds, CancellationToken ct)
    {
        List<int> idlist = orderIds.ToList();
        List<Order> orders;

        await using (var db = await _factory.CreateDbContextAsync(ct))
        {
            orders = await db.Orders.Where(o => idlist.Contains(o.OrderId)).ToListAsync(); //Grab all our orders and pass them to db
        }

        await LoadInventoryCacheAsync(ct);

        var planned = _planner.OrderByPriority(orders); //Put the VIP on the top

        var tasks = planned.Select(id => FulfillOneAsync(id, ct)); //Here the tasks start concurrently

        var results = await Task.WhenAll(tasks); //Here we wait for ALL the orders to be done

        return new BurstResult(
            completed: results.Count(r => r == FulfillmentResult.Completed), //counts how many orderes were completed/Rejected
            rejected: results.Count(r => r == FulfillmentResult.Rejected)
        );
    }

    public async Task LoadInventoryCacheAsync(CancellationToken ct)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        _inventoryCache.Clear();
        var inventoryFromDb = await db.Inventory.ToListAsync(ct);

        foreach(var item in inventoryFromDb)
        {
            _inventoryCache.TryAdd(item.ProductId, item.Quantity);
        }
    }

}

