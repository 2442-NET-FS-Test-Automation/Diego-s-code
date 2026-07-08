using System.IO.Compression;
using coffeShop.Api.Fulfillment;
using coffeShop.data;
using coffeShop.data.Entities;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var conn_string = "Server=localhost,1434;Database=CoffeShop;User Id=sa;Password=Gorka_007!;TrustServerCertificate=true";

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/fulfillment-log-log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

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

app.UseSwagger();
app.UseSwaggerUI();


app.MapGet("/", () => "Hello World!");

app.MapGet("/inventory", async (coffeShopContext db) =>
{
    var inventory = await db.Inventory
            .Include(i => i.Product) //Grab the name of the product also
            .ToListAsync();
    return inventory;
});

app.MapGet("/seed", async (coffeShopContext db) =>
{

   var products = await db.Products.ToListAsync();
   var currentInventory = await db.Inventory.ToListAsync();

   if (!currentInventory.Any())
    {
        foreach(var product in products)
        {
            db.Inventory.Add(new InventoryItem
            {
                ProductId = product.ProductId,
                Quantity = 50
            });
        }
    }
    else
    {
        foreach(var item in currentInventory)
        {
            item.Quantity = 50;
        }
    }
    await db.SaveChangesAsync();
    return Results.Ok(new {message = "Inventory reset 50 unities per product."});
});

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



app.Run();
