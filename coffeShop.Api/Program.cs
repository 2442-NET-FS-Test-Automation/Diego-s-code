using System.Diagnostics;
using System.IO.Compression;
using coffeShop.Api.Fulfillment;
using coffeShop.data;
using coffeShop.data.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

//Database alocated in Docker
var conn_string = "Server=localhost,1434;Database=CoffeShop;User Id=sa;Password=Gorka_007!;TrustServerCertificate=true";

//Logger configuration
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
    .WriteTo.Console()
    .WriteTo.File("logs/fulfillment-log-log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddDbContext<coffeShopContext>(options => options.UseSqlServer(conn_string),
    ServiceLifetime.Scoped, ServiceLifetime.Singleton);

builder.Services.AddDbContextFactory<coffeShopContext>(options => options.UseSqlServer(conn_string));

builder.Services.AddScoped<IFulfillmentService, FulfillmentService>();
builder.Services.AddScoped<ISeeder, Seeder>();
builder.Services.AddScoped<BurstPlaner>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    //I can grab the name of the product from the db and avoid ciclying
   options.SerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles; 
});


var app = builder.Build();

//Use swagger to test my endpoints
app.UseSwagger();
app.UseSwaggerUI();

//The default endpoint
app.MapGet("/", () => "Hello World!");


//Just to see what's in our inventory.
app.MapGet("/inventory", async (coffeShopContext db) =>
{
    var inventory = await db.Inventory
            .Include(i => i.Product) //Grab the name of the product also
            .ToListAsync();
    return inventory;
});


//Here i can seed again our DB with a default stock...
app.MapGet("/seed", async (coffeShopContext db) =>
{

   var products = await db.Products.ToListAsync();
   var currentInventory = await db.Inventory.ToListAsync();

   if (!currentInventory.Any()) //If our db has products
    {
        foreach(var product in products) //Travel across them
        {
            db.Inventory.Add(new InventoryItem
            {
                ProductId = product.ProductId,
                Quantity = 50 //And update the Quantity of all my products to 50
            });
        }
    }
    else
    {
        foreach(var item in currentInventory)
        {
            item.Quantity = 50; //update to 50 all our items in products
        }
    }
    await db.SaveChangesAsync();
    return Results.Ok(new {message = "Inventory reset 50 unities per product."});
});

//Here I Make the orders and give them a priority and a status
app.MapPost("/orders/burst", (int number, bool expedited, ISeeder seeder,
    IServiceScopeFactory scopes, IHostApplicationLifetime lifetime) => 
{
   var ids = seeder.SeedOrders(number, expedited); 
   var StopedApp = lifetime.ApplicationStopped;

   _ = Task.Run( async () =>
   {
       try
       {
            using var scope = scopes.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IFulfillmentService>();
            await service.FulfillBurstAsync(ids, StopedApp);
       }
       catch (Exception ex)
       {
            Log.Error(ex, "Burst failed :c");   
       }
   }, StopedApp); 
    
});

//Test Concurrency against sequential work
app.MapPost("/order/benchmark", async (int number, IFulfillmentService fs, ISeeder seeder, CancellationToken ct) =>
{

    //Sequential First
    var ids1 = seeder.ResetAndCreateOrders(number);
    await fs.LoadInventoryCacheAsync(ct);
    var sw1 = Stopwatch.StartNew();

    foreach(var id in ids1)
    {
        await fs.FulfillOneAsync(id, ct);
    }

    sw1.Stop();

    var ids2 = seeder.ResetAndCreateOrders(number);
    var sw2 = Stopwatch.StartNew();

    Log.Information("Starting Concurrential Orders!");
    Log.Information("Reseting the Stock");

    //Concurrential orders 
    await fs.FulfillBurstAsync(ids2, ct);
    sw2.Stop();

    return new
    {
        sequentialMS = sw1.ElapsedMilliseconds, //Sequential time
        concurrentMS = sw2.ElapsedMilliseconds, //Concurrential Time
        speedUpFactor = Math.Round((double)sw1.ElapsedMilliseconds / sw2.ElapsedMilliseconds, 2) //Difference between them
    };
});

//Reports 
app.MapGet("/Reports/Top-Product", async (coffeShopContext db, CancellationToken ct) =>
{
    var ranked = await db.FulFillmentEvents
        .Where(e => e.Type == "Completed")
        .Join(db.OrderLines, e => e.OrderId, l => l.OrderId, (e, l) => l)
        .GroupBy(l => l.ProductId)
        .Select(g => new {ProductId = g.Key, Units = g.Sum(l => l.Quantity)})
        .OrderByDescending(x => x.Units)
        .ToListAsync(ct);

        return ranked; 
});

app.MapGet("/Reports/Top-Customers", async (coffeShopContext db, CancellationToken ct)=>
{

    var ranked = await db.Orders
        .Where(o => o.Status == "Completed")    
        .GroupBy(o => o.CustomerId)
        .Select(g => new {CustomerId = g.Key, TotalOrders = g.Count()})
        .OrderByDescending(x => x.TotalOrders)
        .Take(3)
        .ToListAsync(ct);

        return ranked;
});

app.MapGet("/Reports/Processing-Time-Rank/{miliseconds:int}", async (int miliseconds, coffeShopContext db, CancellationToken ct)=>
{

    var orders = await db.Orders
        .Where(o => o.Status == "Completed" && o.CompletedAt != null)
        .ToListAsync(ct);

    var processingTime = orders
        .Select(o => (int)(o.CompletedAt.Value - o.CreatedAt).TotalMilliseconds)
        .OrderBy(t => t)
        .ToArray();

        int index = Array.BinarySearch(processingTime, miliseconds);

        if(index >= 0)
    {
        return Results.Ok(new
        {
            Message = "Found it",
            TimeFound = miliseconds,
            RankPosition = index + 1   
        });
    }

    return Results.NotFound($"There wans't any order that took {miliseconds}");
});

app.MapGet("/Reports/Processing-Time-Rank", async (coffeShopContext db, CancellationToken ct)=>
{
    var orders = await db.Orders
        .Where(o => o.Status == "Completed" && o.CompletedAt != null)
        .ToListAsync(ct);

    var processingTime = orders
        .Select(o => (int)(o.CompletedAt.Value - o.CreatedAt).TotalMilliseconds)
        .OrderBy(t => t)
        .ToArray();

    return Results.Ok(processingTime);
});

app.MapPost("/maintenance/reset-all", async (coffeShopContext db) =>
{
    
    await db.Database.ExecuteSqlRawAsync("DELETE FROM FulFillmentEvents");
    await db.Database.ExecuteSqlRawAsync("DELETE FROM OrderLines");
    await db.Database.ExecuteSqlRawAsync("DELETE FROM Orders");

    await db.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Orders', RESEED, 0)");

    await db.SaveChangesAsync();

    return Results.Ok("Database Cleared");

});


//Starts the app

try
{   
    Log.Information("Coffe Shop is now open!");
    app.Run();  
}
catch(Exception ex)
{
    Log.Fatal(ex, "The app just exploted! :c");
}
finally
{
    Log.Information("Closing Coffe Shop");
    Log.CloseAndFlush();
}
