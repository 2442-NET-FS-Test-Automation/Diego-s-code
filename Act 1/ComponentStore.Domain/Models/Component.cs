namespace ComponentStore.Domain;
public abstract class Component
{
 private decimal _Price;
 public decimal Price
    {
        get {return _Price; }
        set
        {
            if (value < 0) _Price = 0;
            else _Price = value;
        }
    }


 public string? Name {get; set;}
 public string SerialNumber {get; set;}    

}
