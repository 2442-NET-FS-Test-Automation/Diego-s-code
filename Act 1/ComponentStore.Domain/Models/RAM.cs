namespace ComponentStore.Domain;

public class RAM : Component
{
    public int Memory {get; set;}

    public RAM(string name, double price, int serialNumber, int memory)
    {
        Name = name;
        Price = price;
        SerialNumber = serialNumber;
        Memory = memory;
    }
    // this is a comment
    //Luis comment

}