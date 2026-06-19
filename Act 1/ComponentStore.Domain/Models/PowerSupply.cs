namespace ComponentStore.Domain;

public class PowerSupply : Component
{
    
    public string Watts;
    public PowerSupply (string name, decimal price, string serialNumber, string watts)
    {
        Name = name;
        Price = price;
        SerialNumber = serialNumber;
        Watts = watts;

    }
}