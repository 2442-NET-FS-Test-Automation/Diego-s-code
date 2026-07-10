public class NotEnoughStockException : Exception
{
    public int ProductId {get;}
    public int Requested {get;}
    public int Available {get;}

    public NotEnoughStockException(int productId, int requested, int available)
            : base($"Not enough stock for product {productId}: requested {requested}, available {available}")
    {
        ProductId = productId;
        Requested  = requested;
        Available = available;
    }

}