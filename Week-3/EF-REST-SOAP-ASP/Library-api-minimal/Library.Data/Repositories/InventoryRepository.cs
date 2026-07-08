using System.Formats.Asn1;
using Library.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Data;

//This class will hold my db acces logic. All is concerned with is looking int the database

public class InventoryRepository : IInventoryRepository
{
    
    //Our repo class needs a db context we can ask for a db context from ASP.NET via container
    //same pattern we've using since day 1 of the minimal API

    private readonly IDbContextFactory<LibraryDbContext> _factory;

    public InventoryRepository(IDbContextFactory<LibraryDbContext> factory)
    {
        _factory = factory;
    }

    //Lets make some CRUD   
    //Actually pretty simple to do - because we don't have to concern ourselves with business logic checks etc.
    //All we write is DB acces stuff
    

    //Let's write some Read methods
    //Get all inventoryItems

    public async Task<IReadOnlyList<InventoryItem>> GetAllAsync()
    {
        //Ask for db context
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Inventory.Include(i => i.Product).ToListAsync();
    }

    //Get an item by it's SKU
    public async Task<InventoryItem?> GetInventoryItemBySkuAsync(string sku)
    {
    
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Inventory.Include(i => i.Product).FirstOrDefaultAsync(i => i.Product.Sku == sku);

    }

    // Lets do a simply add
    //Get in the habit of sending back the newly created objects
    public async Task<InventoryItem> AddInventoryItemAsync(string sku, string name, decimal price, int quantity)
    {
        await using var db = await _factory.CreateDbContextAsync();

        InventoryItem  newItem = new InventoryItem
        {
            Product = new Product {Sku = sku, Name = name, Price = price},
            CurrentStock = quantity
        };

        db.Inventory.Add(newItem);
        await db.SaveChangesAsync();

        return newItem; //because newItem is an object tracked y EF core - EF will grab the PK for us
    }
    
    // Lets do a remove
    public async Task<bool> RemoveSkuAsync(string sku)
    {
        await using var db = await _factory.CreateDbContextAsync();

        //First fing the thing we want out of the database - grab it
        InventoryItem? itemToRemove = await db.Inventory.Include(i => i.Product)
                                            .FirstOrDefaultAsync(i => i.Product.Sku == sku);

        //Don't assume the search criterua produced a result - check for a null
        //If it's null we failed to remove it - because it didn't exist
        if(itemToRemove is null)
        {
            return false;
        }

        //Telling EF we want to remove this object from DB
        db.Products.Remove(itemToRemove.Product); //This SHOULD Cascade based on how we setup our models

        await db.SaveChangesAsync();
        return true;

    }

}