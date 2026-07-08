
using System.IO.Compression;
using coffeShop.data;
using coffeShop.data.Entities;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace coffeShop.Api.Fulfillment;

public interface IFulfillmentService
{

        public Task<FulfillmentResult> FulfillOneAsync(int orderId, CancellationToken ct);
        public Task<BurstResult> FulfillBurstAsync(IEnumerable<int> orderIds, CancellationToken ct);
    
}

public enum FulfillmentResult { Completed, Rejected }

public record BurstResult(int completed, int rejected);

public class FulfillmentService : IFulfillmentService
{
    private readonly IDbContextFactory<coffeShopContext> _factory;
    private readonly BurstPlaner _planner;

    public FulfillmentService(IDbContextFactory<coffeShopContext> factory, BurstPlaner planer)
    {
        _factory = factory;
        _planner = planer;
    }

    public async Task<FulfillmentResult> FulfillOneAsync(int orderId, CancellationToken ct)
    {
        
        await using var db = await _factory.CreateDbContextAsync(ct);
        var order = await db.Orders.Include(o => o.Lines).FirstAsync(o => o.OrderId == orderId, ct);
        var requested = order.Lines.ToDictionary(l => l.ProductId, l => l.Quantity);

        bool canFulfill = true;

        foreach(OrderLine line in order.Lines)
        {
            InventoryItem inv = await db.Inventory.FirstAsync(i => i.ProductId == line.ProductId, ct);
            
            if(inv.Quantity < line.Quantity)
            {
                canFulfill = false;
                break;
            }
            inv.Quantity -= line.Quantity;
        }

        if (!canFulfill)
        {
            order.Status = Status.Rejected.ToString();
            db.FulFillmentEvents.Add(new FulFillmentEvent {
                OrderId = orderId, 
                Type = "Rejected", 
                Message = "Not enough stock"});

            await db.SaveChangesAsync();
            Log.Warning($"Rejected {orderId}: not enought stock", orderId);

            return FulfillmentResult.Rejected;
        }

        order.Status = Status.Completed.ToString();
        order.CompletedAt = DateTime.UtcNow;
        db.FulFillmentEvents.Add(new FulFillmentEvent { OrderId = orderId, Type = "Completed", Message = "Order Complete!"});

        if(!await SaveWithRetryAsync(db, requested, ct))
        {
            db.ChangeTracker.Clear();
            Order staleOrder = await db.Orders.FirstAsync(o => o.OrderId == orderId, ct);
            staleOrder.Status = Status.Rejected.ToString();
            Log.Warning("Rejected order {OrderId} after concurrency retry", orderId);
            return FulfillmentResult.Rejected;
        }

        Log.Information("Fulfilled order: {OrderId}, {LineCount} lines", orderId, order.Lines.Count);
        return FulfillmentResult.Completed;


    }

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

                    if(current is null) return false;

                    entry.OriginalValues.SetValues(current);

                    if(entry.Entity is InventoryItem inv)
                    {
                        int newValue = current.GetValue<int> (nameof(InventoryItem.Quantity));
                        int desiredAmount = requestedByProductId[inv.ProductId];
                        if(newValue < desiredAmount) return false;
                        inv.Quantity = newValue - desiredAmount;
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
            orders = await db.Orders.Where(o => idlist.Contains(o.OrderId)).ToListAsync();
        }

        var planned = _planner.OrderByPriority(orders);

        var tasks = planned.Select(id => FulfillOneAsync(id, ct));

        var results = await Task.WhenAll(tasks);

        return new BurstResult(
            completed: results.Count(r => r == FulfillmentResult.Completed),
            rejected: results.Count(r => r == FulfillmentResult.Rejected)
        );
    }

}

