using Microsoft.EntityFrameworkCore;
using coffeShop.data.Entities;
using System.IO.Compression;

namespace coffeShop.data;

public class coffeShopContext : DbContext
{
    public coffeShopContext(DbContextOptions<coffeShopContext> options) : base(options) {} 

    public DbSet<Product> Products => Set<Product>();
    public DbSet<InventoryItem> Inventory => Set<InventoryItem>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderLine> OrderLines => Set<OrderLine>();
    public DbSet<FulFillmentEvent> FulFillmentEvents => Set<FulFillmentEvent>();


    protected override void OnModelCreating(ModelBuilder b)
    {


     //Product entities:   
        b.Entity<Product>(e =>
        {
            e.HasIndex(p => p.Sku).IsUnique();
            e.Property(p => p.Price).HasColumnType("decimal(10, 2)");
            e.HasOne(p => p.Inventory)
                .WithOne(i => i.Product)
                .HasForeignKey<InventoryItem>(i => i.ProductId);
        });

    //FulFillment entities:
        b.Entity<FulFillmentEvent>(e =>
        {
            e.HasKey(f => f.FulFillmentEventId);
            e.Property(f => f.Message).HasMaxLength(250);
            e.Property(f => f.Type).HasMaxLength(50);
            e.Property(f => f.TimeStamp).HasDefaultValueSql("GETDATE()");
            e.HasOne(f => f.Order)
                .WithMany()
                .HasForeignKey(f => f.OrderId);

        });

    //InventoryItem entities:
        b.Entity<InventoryItem>(e =>
        {
            e.HasKey(i => i.ItemId);
            e.Property(i =>  i.RowVersion).IsRowVersion();
            e.Property(i => i.Quantity).HasDefaultValue(0);
        });

        //Customer entities:
        b.Entity<Customer>(e =>
        {
            e.Property(c => c.Email).HasMaxLength(256);
            e.HasIndex(c => c.Email).IsUnique();
        });

        //Order entities:
        b.Entity<Order>(e =>
        {
            e.Property(o => o.CreatedAt).HasDefaultValueSql("GETDATE()");
            e.Property(o => o.Status).HasConversion<string>().HasMaxLength(30);
            e.Property(o => o.Priority).IsRequired();
        });


        //Seed data:

        //Drinks
        b.Entity<Product>().HasData(
            //Hot drinks
            new Product {ProductId = 1, Sku = "HOT-AME-01",Name = "American", Price = 50.00m},
            new Product {ProductId = 2, Sku = "HOT-LAT-02",Name = "Latte", Price = 65.00m},
            new Product {ProductId = 3, Sku = "HOT-CAP-03",Name = "Capuccino", Price = 60.00m},
            new Product {ProductId = 4, Sku = "HOT-TAR-04",Name = "Taro", Price = 80.00m},
            new Product {ProductId = 5, Sku = "HOT-CHA-05",Name = "Natural Chai", Price = 90.00m},
            //Cold drinks
            new Product {ProductId = 6, Sku = "COL-LAT-06",Name = "Iced Latte", Price = 70.00m},
            new Product {ProductId = 7, Sku = "COL-AME-07",Name = "Iced American", Price = 60.00m},
            new Product {ProductId = 8, Sku = "COL-TAR-08",Name = "Iced Taro", Price = 85.00m},
            new Product {ProductId = 9, Sku = "COL-CHA-09",Name = "Iced Chai", Price = 95.00m}
        );
        //Customers
        b.Entity<Customer>().HasData(
            new Customer {CustomerId = 1, Name = "Daniel", Email = "example1@gmail.com"},
            new Customer {CustomerId = 2, Name = "Ana", Email = "example2@gmail.com"},
            new Customer {CustomerId = 3, Name = "Valeria", Email = "example3@gmail.com"},
            new Customer {CustomerId = 4, Name = "Deniro", Email = "example4@gmail.com"},
            new Customer {CustomerId = 5, Name = "Nahomi", Email = "example5@gmail.com"},
            new Customer {CustomerId = 6, Name = "Diego", Email = "example6@gmail.com"},
            new Customer {CustomerId = 7, Name = "Fernanda", Email = "example7@gmail.com"}
        );
    }
}
