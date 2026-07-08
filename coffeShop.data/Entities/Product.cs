using Microsoft.EntityFrameworkCore;

namespace coffeShop.data.Entities;

public class Product
{
    public int ProductId {get; set;}
    public string Sku {get; set;}
    public string Name {get; set;}

    [Precision(10, 2)]
    public decimal Price {get; set;}
    
    public InventoryItem? Inventory {get; set;}
}