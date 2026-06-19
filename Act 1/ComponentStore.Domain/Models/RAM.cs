namespace ComponentStore.Domain;

public class RAM : Component
{
    public int Memory {get; private set;}

    public RAM(string serialNumber, string name, decimal price, int memory) : base(serialNumber, name, price)
    {
        Memory = memory;
    }

    public override string Describe()
    {
        return $"{SerialNumber}: {Name} has {Memory} and its price is {Price}";
    }


}