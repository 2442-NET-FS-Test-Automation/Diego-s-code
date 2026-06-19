using System.Diagnostics;

namespace ComponentStore.Domain;

public class Processor : Component
{
    public double ClockSpeed {get; private set;}

    public Processor(string serialNumber, String name, decimal price, double clockSpeed, uint stock) : base(serialNumber, name, price, stock)
    {
        ClockSpeed = clockSpeed;

    }

    public override string Describe()
    {
        return $"{SerialNumber}: {Name} has {ClockSpeed} of clock speed and its price is {Price}";
    }
}