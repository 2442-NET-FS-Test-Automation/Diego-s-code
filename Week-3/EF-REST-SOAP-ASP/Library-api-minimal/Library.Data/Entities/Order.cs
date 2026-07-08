namespace Library.Data.Entities;

public class Order
{
    
    public int Id {get; set;}
    public int CustomerId {get; set;} //FK -> Customer
    public Customer Customer {get; set;} = default!;
    public Priority Priority {get; set;}
    public Status Status{get; set;}
    public DateTime CreatedUtc {get; set;} = DateTime.UtcNow; //Stamp it upon object creation
    public DateTime? CompletedUtc {get; set;}

    //Every order has one or more orderlines
    //OrderLines are the actual product and quantity of something on the order    
    public List<OrderLine> Lines {get; set;} = new();

}
