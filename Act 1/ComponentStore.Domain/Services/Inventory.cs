namespace ComponentStore.Domain;

public class Inventory
{
    
    private List<Component> _items = new List<Component>();

    public Inventory()
    {
        _items.Add(new Processor("DFE3453","AMD Ryzen 7", 249, 5, 10, SocketType.AM4));
        _items.Add(new GraphicCard("EG3422", "RTX 5090TI", 999, 12, 8));
        _items.Add(new RAM("34345","Corsair Vengeance Pro", 49, 2, 16, MemoryType.DDR3));
    }

    public void ListItems()
    {
        Console.WriteLine("== This are the components in your inventory ==");

        if(_items.Count == 0)
        {
            Console.WriteLine("There is no components in your inventory, you need to add some, maybe...");
            return;
        }

        for(int i = 0; i < _items.Count; i++)
        {
            var Piece = _items[i];
            int menuNumber = i + 1;

            if(Piece is GraphicCard TemporalGraphicCard)
            {
                 Console.WriteLine($"{menuNumber}. Name: {Piece.Name}, Price: {Piece.Price}, S/N: {Piece.SerialNumber}, Vram: {TemporalGraphicCard.Vram}");
            }
            else if(Piece is Processor TemporalProcessor)
            {
                Console.WriteLine($"{menuNumber}. Name: {Piece.Name}, Price: {Piece.Price}, S/N: {Piece.SerialNumber}, Clock Speed: {TemporalProcessor.ClockSpeed}, Socket: {TemporalProcessor.Socket}");
            }
            else if(Piece is RAM TemporalRAM)
            {
                Console.WriteLine($"{menuNumber}. Name: {Piece.Name}, Price: {Piece.Price}, S/N: {Piece.SerialNumber}, Memory: {TemporalRAM.Memory}");
            }
            else if(Piece is MotherBoard TemporalMB)
            {
                Console.WriteLine($"{menuNumber}. Name: {Piece.Name}, Price: {Piece.Price}, S/N: {Piece.SerialNumber}, Stock: {Piece.Stock}, Socket: {TemporalMB.Socket}, Memory type: {TemporalMB.Memory}");
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
        public Component GetComponent(int choice)
    {
        if (choice > 0 && choice <= _items.Count)
        {
            return _items[choice - 1];
        }

        Console.WriteLine("\n [Error], That number doesn't exist");
        return null;
    }


}