using System.Diagnostics;

namespace ComponentStore.Domain;

public class Processor : Component
{
    public double ClockSpeed {get; set;}

    public Processor (String name, decimal price, string serialNumber, double clockSpeed)
    {
        Name = name;
        Price = price;
        SerialNumber = serialNumber;
        ClockSpeed = clockSpeed;
        
    }
}