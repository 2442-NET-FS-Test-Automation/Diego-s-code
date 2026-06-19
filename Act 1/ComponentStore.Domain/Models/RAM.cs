namespace ComponentStore.Domain;

public class RAM : Component, interfaceRam
{
    public int MemoryCapacity { get; set; }
    public MemoryType Memory { get; }

    public RAM(string serialNumber, string name, decimal price, uint stock, int memoryCapacity, MemoryType memory) : base(serialNumber, name, price, stock)
    {
        MemoryCapacity = memoryCapacity;
        Memory = memory;
    }

    public override string Describe()
    {
        return $"{SerialNumber}: {Name} its type is {Memory}, its memory capacity is {MemoryCapacity} and its price is {Price}";
    }
}