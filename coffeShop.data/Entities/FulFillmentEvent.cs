namespace coffeShop.data.Entities;

public class FulFillmentEvent
{
    public int FulFillmentEventId {get; set;}
    public int OrderId {get; set;}
    public Order Order {get; set;} = default!;
    public string Type {get; set;}
    public string Message {get; set;}
    public DateTime TimeStamp {get; set;}
    
}