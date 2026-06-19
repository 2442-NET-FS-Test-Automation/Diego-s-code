using System.Diagnostics;

namespace ComponentStore.Domain;

public class Processor : Component, InterfaceCPU
{

    public SocketType Socket {get; }
    public double ClockSpeed {get; set;}

    public Processor (String name, decimal price, string serialNumber, double clockSpeed, SocketType socket)
    {
        Name = name;
        Price = price;
        SerialNumber = serialNumber;
        ClockSpeed = clockSpeed;
        Socket = socket;
        
    }
}