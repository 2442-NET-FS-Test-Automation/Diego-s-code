using coffeShop.data;
using coffeShop.data.entities;
using coffeShop.data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace coffeShop.Api.Fulfillment;

public interface ISeeder
{
    IReadOnlyList<int> SeedOrders(int n, bool expedited);
    IReadOnlyList<int> ResetAndCreateOrders(int n);
}

public class Seeder : ISeeder
{
    private static readonly string[] Skus = {"HOT-AME-01", "HOT-LAT-02", "HOT-CAP-03", "HOT-TAR-04",
                                             "HOT-CHA-05", "COL-LAT-06", "COL-AME-07", "COL-TAR-08",
                                             "COL-CHA-09"};

    private readonly IDbContextFactory<coffeShopContext> _factory;
    public Seeder (IDbContextFactory<coffeShopContext> factory)
    {
        _factory = factory;
    }


    public IReadOnlyList<int> SeedOrders (int n, bool expedited)
    {
        //I ask for a db context
        using var db = _factory.CreateDbContext();

        //Then I Create a dictionary 
        var pid = db.Products.ToDictionary(p => p.Sku, p => p.ProductId); 

        var ids = new List<int> (n);

        for (int i = 0; i < n; i++)
        {
            var order = new Order
            {
              CustomerId = Random.Shared.Next(1, 8),
              Priority = expedited ? Priority.Expedited : Priority.Normal,
              Lines = {new OrderLine {ProductId = Random.Shared.Next(1, 10), Quantity = 1}},
              Status = coffeShop.data.Entities.Status.Pending.ToString()
            };

            db.Orders.Add(order);
            db.SaveChanges();   
            ids.Add(order.OrderId);
            
        }

        return ids;
    }

    public IReadOnlyList<int> ResetAndCreateOrders (int n)
    {
        
        using var db = _factory.CreateDbContext();

        foreach (InventoryItem inv in db.Inventory)
        {
            if(inv.Quantity != 50) //If my stock is different from 50
            {
                inv.Quantity = 50; //update them to be the default value
            }
        }

        db.SaveChanges();

        var pid = db.Products.ToDictionary(p => p.Sku, p => p.ProductId);

        var ids = new List<int>(n);

        for(var i = 0; i < n; i++)
        {
            var order = new Order
            {
                CustomerId = Random.Shared.Next(1, 8), //Pick a random customer
                Priority = i % 3 == 0 ? Priority.Expedited : Priority.Normal, //A chance to be priority or normal order
                Lines = {new OrderLine {ProductId = Random.Shared.Next(1, 10), Quantity = 1}}  //Pick a random drink in the db
            };

            db.Orders.Add(order);
            db.SaveChanges();   
            ids.Add(order.OrderId);
        }

        return ids;

    }
}