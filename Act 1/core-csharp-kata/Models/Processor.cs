using System.Diagnostics;

namespace Models;

public class Processor : Component
{
    public double ClockSpeed {get; set;}

    public Processor (String name, double price, int serialNumber, double clockSpeed)
    {
        Name = name;
        Price = price;
        SerialNumber = serialNumber;
        ClockSpeed = clockSpeed;
        
    }
}