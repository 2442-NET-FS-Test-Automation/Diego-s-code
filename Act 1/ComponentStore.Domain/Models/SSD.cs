namespace ComponentStore.Domain;

public class SSD : Component
{
    
        public int Storage;
    public SSD (string name, decimal price, string serialNumber, int storage)
    {
        Name = name;
        Price = price;
        SerialNumber = serialNumber;
        Storage = storage;

    }
}