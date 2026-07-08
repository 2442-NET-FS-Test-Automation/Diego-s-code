namespace coffeShop.data.Entities;

public class OrderLine
{
    public int OrderLineId {get; set;}
    public int OrderId {get; set;}
    public Order Order {get; set;} = default!;
    public int ProductId {get; set;}
    public Product Product {get; set;} = default!;
    public int Quantity {get; set;}

}