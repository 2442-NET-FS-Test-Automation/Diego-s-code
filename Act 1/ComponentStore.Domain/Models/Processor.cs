using System.Diagnostics;

namespace ComponentStore.Domain;

public class Processor : Component, InterfaceCPU
{

    public double ClockSpeed {get; private set;}
    public SocketType Socket {get; }
    public Processor(string serialNumber, String name, decimal price, double clockSpeed, uint stock, SocketType socket) : base(serialNumber, name, price, stock)
    {
        ClockSpeed = clockSpeed;
        Socket = socket;
    }

    public override string Describe()
    {
        return $"{SerialNumber}: {Name} has {ClockSpeed} of clock speed and its price is {Price}";
        
    }
}