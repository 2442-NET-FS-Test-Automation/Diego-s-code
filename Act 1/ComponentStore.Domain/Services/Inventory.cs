namespace ComponentStore.Domain;

public class Inventory
{
    
    private List<Component> _items = new List<Component>();

    public Inventory()
    {
        _items.Add(new Processor("AMD Ryzen 7", 249, "123456789", 199.99));
        _items.Add(new GraphicCard("RTX 5090TI", 999, "123456789", 12));
        _items.Add(new RAM("Corsair Vengeance Pro", 49, "123456789", 16));
    }

    public void ListItems()
    {
        Console.WriteLine("== This are the components in your inventory ==");

        if(_items.Count == 0)
        {
            Console.WriteLine("There is no components in your inventory, you need to add some, maybe...");
            return;
        }

        for(int i = 1; i < _items.Count; i++)
        {
            var Piece = _items[i];

            if(Piece is GraphicCard TemporalGraphicCard)
            {
                 Console.WriteLine($"{i}. Name: {Piece.Name}, Price: {Piece.Price}, S/N: {Piece.SerialNumber}, Vram: {TemporalGraphicCard.Vram}");
            }
            else if(Piece is Processor TemporalProcessor)
            {
                Console.WriteLine($"{i}. Name: {Piece.Name}, Price: {Piece.Price}, S/N: {Piece.SerialNumber}, Clock Speed: {TemporalProcessor.ClockSpeed}");
            }
            else if(Piece is RAM TemporalRAM)
            {
                Console.WriteLine($"{i}. Name: {Piece.Name}, Price: {Piece.Price}, S/N: {Piece.SerialNumber}, Memory: {TemporalRAM.Memory}");
            }
           
        }
    }

    public void AddItem(Component newItem)
    {
        _items.Add(newItem);
        Console.WriteLine($"The component {newItem.Name} was succesfully added to your inventory!");
    }

    public void RemoveItem(int choice)
    {
        int realIndex = choice - 1;

        if(realIndex >= _items.Count || realIndex < 0)
        {
         Console.WriteLine("That number doesn't exist! Try again");   
         return;
        }
        else
        {
            _items.RemoveAt(realIndex);
            Console.WriteLine("Items were removed succesfully!");
        }
        
    }
}