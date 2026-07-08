namespace Library.Api.Fulfillment;

public sealed class UnknowSkuExpetion : Exception
{
    public string Sku{get; }

    public UnknowSkuExpetion(string sku) : base($"Unknow SKU: {sku}")
    {
        Sku = sku;
    }
 }