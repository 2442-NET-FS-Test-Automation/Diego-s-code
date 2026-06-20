using System.ComponentModel.Design.Serialization;
using System.Net;

using ComponentStore.Domain;
using ComponentStore.Infrastructure;
using Serilog;

public class Program
{
    static Inventory MyInventory = new Inventory();

    public static void Main()
    {
        LoggerConfig.Configure();

        GlobalExceptionHandler.Register();

        try
        {
            RunApplication();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    static void RunApplication()
    {
        var running = true;


        while (running)
        {
            PrintMenu();
            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("That is not a number!");

                continue;
            }
            switch (choice)
            {
                case 1: AddComponent(); break;
                case 2: MyInventory.ListItems(); break;
                case 3: DeleteComponent(); break;
                case 0: running = false; break;

            }
        }
    }


    static void PrintMenu()
    {
        Console.WriteLine("===Welcome to your PC components List!===");
        Console.WriteLine("Select any option");
        Console.WriteLine("1. Add a component");
        Console.WriteLine("2. See your components");
        Console.WriteLine("3. Delete a component");
        Console.WriteLine("0. Exit the app");

    }

    static void AddComponent()
    {
        Console.WriteLine("\nWhat do you want to add to your collection?");
        Console.WriteLine("1. Processor");
        Console.WriteLine("2. Graphic Card");
        Console.WriteLine("3. RAM");

        int choice = int.Parse(Console.ReadLine());
        switch (choice)
        {
            case 1: ProcessorAdd(); break;
            case 2: GraphicCardAdd(); break;
            case 3: RAMAdd(); break;
            case 0: break;
        }
    }

    static void DeleteComponent()
    {

        Console.WriteLine("Select your component to delete it!");
        MyInventory.ListItems();
        Console.WriteLine("Write down the number you want to delete (Be carefull)");
        int choice = int.Parse(Console.ReadLine());
        MyInventory.RemoveItem(choice);
    }

   static void ProcessorAdd()
    {

        var (name, price, serialNumber) = GeneralData();

        Console.WriteLine(" --- Now Clock Speed");
        double clockSpeed = double.Parse(Console.ReadLine());

        Processor NewProcessor = new Processor (name, price, serialNumber, clockSpeed)
        {
            Name = name,
            Price = price,
            SerialNumber = serialNumber,
            ClockSpeed = clockSpeed

        };

        MyInventory.AddItem(NewProcessor);
        Console.WriteLine("\nProcessor added!");

    }

    static void GraphicCardAdd()
    {
        var (name, price, serialNumber) = GeneralData();

        Console.WriteLine(" --- Now VRAM capacity");
        int vram = int.Parse(Console.ReadLine());

        
        GraphicCard NewGraphicCard = new GraphicCard (name, price, serialNumber, vram)
        {
            Name = name,
            Price = price,
            SerialNumber = serialNumber,
            Vram = vram

        };

        MyInventory.AddItem(NewGraphicCard);
        Console.WriteLine("\nGraphic Card added!");

        
    }

    static void RAMAdd()
    {
         var (name, price, serialNumber) = GeneralData();

        Console.WriteLine(" --- Now Memory capacity");
        int memory = int.Parse(Console.ReadLine());

        
        RAM NewRam = new RAM (name, price, serialNumber, memory)
        {
            Name = name,
            Price = price,
            SerialNumber = serialNumber,
            Memory = memory

        };

        MyInventory.AddItem(NewRam);
        Console.WriteLine("\nRAM added!");
        
    }

    static (string name, decimal price, string serialNumber) GeneralData()
    {
        
        Console.WriteLine(" --- Write Down the name and model!");
        string name =Console.ReadLine();

        Console.WriteLine(" --- Now give it a price");
        decimal price = decimal.Parse(Console.ReadLine());

        Console.WriteLine(" --- Now Serial Number");
        string serialNumber = Console.ReadLine();

        return (name, price, serialNumber);
    }


}