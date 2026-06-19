namespace ComponentStore.Domain;


public class GraphicCard : Component
    
{
    public int? Vram {get; set;}
    public GraphicCard(string name, decimal price, string serialNumber, int vram)
    {
     Name = name;
     Price = price;
     SerialNumber = serialNumber;
     Vram = vram;
    }
}