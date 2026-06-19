namespace ComponentStore.Domain;

public class PowerSupply : Component
{
    
    public string Watts;
    public PowerSupply (string serialNumber, string name, decimal price, uint stock, string watts) : base(serialNumber, name, price, stock)
    {
        Watts = watts;
    }

    public override string Describe()
    {
        return $"{SerialNumber}: {Name} is {Watts} of watts and its price is{Price}";
    }
}