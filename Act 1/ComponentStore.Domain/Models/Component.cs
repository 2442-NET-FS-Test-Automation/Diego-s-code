namespace ComponentStore.Domain;

public abstract class Component
{
    private decimal _price;
    public decimal Price
    {
        get { return _price; }
        set
        {
            if (value < 0) _price = 0;
            else _price = value;
        }
    }


    public string? Name { get; set; }
    public string SerialNumber { get; set; }


    protected Component(string serialNumber, string name, decimal price)
    {
        this.SerialNumber = serialNumber;
        this.Name = name;
        this.Price = price;
    }

    public abstract string Describe();

    public override string ToString() => Describe();
}
