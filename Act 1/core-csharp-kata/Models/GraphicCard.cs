namespace Models;


public class GraphicCard : Component
    
{
    public int? Vram {get; set;}
    public GraphicCard(string name, double price, int serialNumber, int vram)
    {
     Name = name;
     Price = price;
     SerialNumber = serialNumber;
     Vram = vram;
    }
}