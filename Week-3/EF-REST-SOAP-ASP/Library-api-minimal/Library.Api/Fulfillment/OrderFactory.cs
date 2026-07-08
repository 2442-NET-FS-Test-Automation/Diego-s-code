namespace Library.Api.Fulfillment;

using Library.Data.Entities;

public class OrderFactory
{
    
    private readonly IFulfillmentService _fs;

    public OrderFactory(IFulfillmentService fulfillment)
    {
        _fs = fulfillment;
    }

    public Order CreateOrder(String kind, int customerId, IEnumerable<(string sku, int qty)> lines)
    {
        switch (kind)
        {
            case "normal":
                return BuildOrder(Priority.Normal, customerId, lines);
            
            case "expedited":
                return BuildOrder(Priority.Expedited, customerId, lines);
            
            default:
                throw new ArgumentException($"Unknow order kind: {kind}");
        }
    }

    private Order BuildOrder(Priority priority, int customerId, IEnumerable<(string sku, int qty)> lines)
    {
        return new Order
        {
            CustomerId = customerId,
            Priority = priority,
            Status = Status.Pending,
            Lines = lines.Select(l => new OrderLine
            {
                ProductId = _fs.ResolveProductId(l.sku), //unknow SKU => UnknowSkuException
                Quantity = l.qty
            }).ToList()
        };
    }

}