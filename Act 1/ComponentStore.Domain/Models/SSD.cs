namespace ComponentStore.Domain;

public class SSD : Component
{
    public int Storage;
    public SSD (string serialNumber, string name, decimal price, uint stock, int storage) : base(serialNumber, name, price, stock)
    {
        Storage = storage;
    }

    public override string Describe()
    {
        return $"";
    }
}