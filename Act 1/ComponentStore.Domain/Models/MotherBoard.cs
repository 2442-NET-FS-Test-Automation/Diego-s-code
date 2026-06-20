namespace ComponentStore.Domain;

public class MotherBoard : Component, InterfaceMB
{
    public SocketType Socket {get; private set;}
    public MemoryType Memory {get; private set;}

    public MotherBoard (string serialNumber, string name, decimal price, uint stock,  SocketType socket, MemoryType memory) : base(serialNumber, name, price, stock)
    {
        Socket = socket;
        Memory = memory;
    }

    public override string Describe()
    {
        return $"{SerialNumber}: {Name} its socket is {Socket}, its memory type is {Memory} and its price is{Price}";
    }
}