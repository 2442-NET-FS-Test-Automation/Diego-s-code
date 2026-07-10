
using System.Runtime.InteropServices.Swift;
using coffeShop.data.entities;
using coffeShop.data.Entities;

namespace coffeShop.Api.Fulfillment;

public class OrderFactory
{
    
    private readonly IFulfillmentService _fs;

    public OrderFactory(IFulfillmentService fulfillment)
    {
        _fs = fulfillment;
    }

    //I grab the order and se if it is a normal or a vip one
    public Order CreateOrder(string typo, int customerId, IEnumerable<(string sku, int quantity)> lines)
    {
        switch (typo)
        {
            case "normal":
                return BuildOrder(Priority.Normal, customerId, lines);
            case "vip":
                return BuildOrder(Priority.VIP, customerId, lines);
            default:
                throw new ArgumentException($"Unknow order tipe: {typo}");
        }
    }

    //Here I create my order to send it to my db
    public Order BuildOrder(Priority priority, int customerId, IEnumerable<(string sku, int quantity)> lines)
    {

        //Return my order with the data
        return new Order
        {
            CustomerId = customerId, //The Id of the customer
            Priority = priority,    //It's priority
            Status = Status.Pending.ToString(), //set the status pending
            Lines = lines.Select(l => new OrderLine  
            {  
                ProductId = _fs.ResolveProductId(l.sku), 
                Quantity = l.quantity    
            }).ToList() //And send it to a list
        };

    }

}