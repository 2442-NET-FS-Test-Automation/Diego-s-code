namespace ComponentStore.Domain;

public class RAM : Component, interfaceRam
{
    public MemoryType Memory {get; }
    public int MemoryCapacity {get; set;}

    public RAM(string name, decimal price, string serialNumber, int memoryCapacity, MemoryType memory)
    {
        Name = name;
        Price = price;
        SerialNumber = serialNumber;
        MemoryCapacity = memoryCapacity;
        Memory = memory;
    }
    // this is a comment
    //Luis comment

}