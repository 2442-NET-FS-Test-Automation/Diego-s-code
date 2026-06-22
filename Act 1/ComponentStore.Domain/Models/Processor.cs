using System.Diagnostics;

namespace ComponentStore.Domain;

public class Processor : Component, InterfaceCPU
{

    public double ClockSpeed {get; set;}
    public SocketType Socket {get; }
    public Processor(string serialNumber, String name, decimal price, uint stock, double clockSpeed, SocketType socket) : base(serialNumber, name, price, stock)
    {
        ClockSpeed = clockSpeed;
        Socket = socket;
    }

    public override string Describe()
    {
        return $"{SerialNumber}: {Name} has {ClockSpeed} of clock, its socket type {Socket} speed and its price is {Price}";
        
    }
}