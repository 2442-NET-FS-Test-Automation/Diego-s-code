namespace ComponentStore.Domain;

public class MotherBoard : Component, InterfaceMB
{
    public SocketType Socket {get; private set;}
    public MemoryType Memory {get; private set;}

    public MotherBoard (string name, decimal price, string serialNumber, SocketType socket, MemoryType memory)
    {
        Name = name;
        Price = price;
        SerialNumber = serialNumber;
        Socket = socket;
        Memory = memory;

    }

}