namespace ComponentStore.Domain;
public abstract class Component
{
 private double _Price;
 public double Price
    {
        get {return _Price; }
        set
        {
            if (value < 0) _Price = 0;
            else _Price = value;
        }
    }


 public string? Name {get; set;}
 public int SerialNumber {get; set;}    

}
