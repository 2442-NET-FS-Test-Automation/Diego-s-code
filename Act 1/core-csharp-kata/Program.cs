using System.ComponentModel.Design.Serialization;
using System.Net;
using Models;
using Services;
public class Program
{
    public static void Main()
    {
        var running = true;
        var MyInventory = new Inventory();
    
        while (running)
        {
            PrintMenu();
            int choice = int.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1: AddComponent(); break;
                case 2: MyInventory.ListItems(); break;
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

   static void ProcessorAdd()
    {
        Console.WriteLine(" --- ");
    }

    static void GraphicCardAdd()
    {
        
    }

    static void RAMAdd()
    {
        
    }


}