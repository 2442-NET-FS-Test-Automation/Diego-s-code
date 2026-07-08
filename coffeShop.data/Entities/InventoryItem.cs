namespace coffeShop.data.Entities;

public class InventoryItem
{
    public int ItemId {get; set;}
    public int ProductId {get; set;}
    public Product Product {get; set;} = default!;
    public int Quantity {get; set;}
    public byte[] RowVersion {get; set;} = default!;
    
}