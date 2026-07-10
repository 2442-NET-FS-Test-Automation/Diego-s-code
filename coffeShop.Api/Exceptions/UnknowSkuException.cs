
namespace coffeShop.Api.Exceptions;

//My custom exception when an SKU doesn't exist in my DB
public sealed class UnknowSkuException : Exception
{
    public string Sku {get; }

    public UnknowSkuException(string sku ) : base($"Unknow SKU: {sku}")
    {
        Sku = sku;
    }
}