namespace ComponentStore.Domain;

public class Inventory
{
    
    private List<Component> _items = new List<Component>();

    public Inventory()
    {
        _items.Add(new Processor("AMD Ryzen 7", 249.99, 123456789, 199.99));
        _items.Add(new GraphicCard("RTX 5090TI", 999, 123456789, 12));
        _items.Add(new RAM("Corsair Vengeance Pro", 49.99, 123456789, 16));
    }

    public void ListItems()
    {
        Console.WriteLine("== This are the components in your inventory ==");

        if(_items.Count == 0)
        {
            Console.WriteLine("There is no components in your inventory, you need to add some, maybe...");
            return;
        }

        foreach(var item in _items)
        {
            Console.WriteLine($"S/N: {item.SerialNumber} | Component: {item.Name} | Price: {item.Price}");
        }
    }

    public void AddItem(Component newItem)
    {
        _items.Add(newItem);
        Console.WriteLine($"");
    }

    public void RemoveItem(Component index)
    {
        
    }
}