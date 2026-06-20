namespace ComponentStore.Domain;


public class GraphicCard : Component
    
{
    public int? Vram {get; private set;}
    public GraphicCard(string serialNumber, string name, decimal price, uint stock, int vram) : base(serialNumber, name, price, stock)
    {
     Vram = vram;
    }

    public override string Describe()
    {
        return $"{SerialNumber}: {Name} has {Vram} of Vram and its price is{Price}";
    }
}