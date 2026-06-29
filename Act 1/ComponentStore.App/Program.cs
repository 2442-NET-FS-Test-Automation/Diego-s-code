using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Net;
using System.Runtime.InteropServices.Marshalling;
using ComponentStore.Domain;
using ComponentStore.Infrastructure;
using Serilog;

public partial class Program
{
    static Inventory MyInventory = new Inventory();
    static BuildServices MyServices = new BuildServices();


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
                case 4: CreateConfig(); break;
                case 5: AddComponentToPc(); break;
                case 6: ShowPCDetails(); break;
                case 7: RemoveComponentFromConfig(); break;
                case 0: running = false; break;

            }
            
            ClearConsole();
        }
    }
}