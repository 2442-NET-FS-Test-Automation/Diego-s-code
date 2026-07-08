using coffeShop.data.entities;

namespace coffeShop.data.Entities;

public class Order
{
    
    public int OrderId {get; set;}
    public int CustomerId {get; set;}
    public Customer Customer {get; set;} = default!;
    public string Status {get; set;}
    public Priority Priority {get; set;}
    public DateTime CreatedAt {get; set;}
    public DateTime? CompletedAt {get; set;}

    public List<OrderLine> Lines {get; set;} = new();
    
}