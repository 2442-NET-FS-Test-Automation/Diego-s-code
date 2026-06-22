using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Net;
using System.Runtime.InteropServices.Marshalling;
using ComponentStore.Domain;
using ComponentStore.Infrastructure;
using Serilog;

public class Program
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
        }
    }


    static void PrintMenu()
    {
        Console.WriteLine("===Welcome to your PC components List!===");
        Console.WriteLine("Select any option");
        Console.WriteLine("1. Add a component");
        Console.WriteLine("2. See your components");
        Console.WriteLine("3. Delete a component");
        Console.WriteLine("4. Create a Configuration");
        Console.WriteLine("5. Add components to you config"); 
        Console.WriteLine("6. See Your Configurations");
        Console.WriteLine("7. Edit configuration");
        Console.WriteLine("0. Exit the app");

    }

    static void AddComponent()
    {
        Console.WriteLine("\nWhat do you want to add to your collection?");
        Console.WriteLine("1. Processor");
        Console.WriteLine("2. Graphic Card");
        Console.WriteLine("3. RAM");
        Console.WriteLine("4. Mother Board");
        Console.WriteLine("0. Return");

        int choice = int.Parse(Console.ReadLine());
        switch (choice)
        {
            case 1: ProcessorAdd(); break;
            case 2: GraphicCardAdd(); break;
            case 3: RAMAdd(); break;
            case 4: MBAdd(); break;
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

        var (name, price, serialNumber, stock) = GeneralData();

        Console.WriteLine(" --- Now Clock Speed");
        double clockSpeed = double.Parse(Console.ReadLine());

        Console.WriteLine("Wich socket it uses?");
        Console.WriteLine("1. AM4");
        Console.WriteLine("2. AM5");
        Console.WriteLine("3. LGA1151");
        Console.WriteLine("4. LGA1200");
        Console.WriteLine("5. LGA1700");

        
        int socketType;

        
        while(!int.TryParse(Console.ReadLine(), out socketType) || socketType < 1 || socketType > 5)
        {
            Console.WriteLine("Invalid Option, Please Select between 1 and 5: ");
        }

        SocketType SelectedSocket = SocketType.LGA1151;

        if(socketType == 1)
        {
            SelectedSocket = SocketType.AM4;
        }
        else if(socketType == 2)
        {
            SelectedSocket = SocketType.AM5;
        }
        else if(socketType == 3)
        {
            SelectedSocket = SocketType.LGA1151;
        }
        else if(socketType == 4)
        {
            SelectedSocket = SocketType.LGA1200;
        }
        else if(socketType == 5)
        {
            SelectedSocket = SocketType.LGA1700;
        }

        Processor NewProcessor = new Processor (serialNumber, name, price, stock, clockSpeed, SelectedSocket)
        {
            Name = name,
            Price = price,
            SerialNumber = serialNumber,
            Stock = stock,
            ClockSpeed = clockSpeed

        };

        MyInventory.AddItem(NewProcessor);
        Console.WriteLine("\nProcessor added!");

    }

    static void GraphicCardAdd()
    {
        var (name, price, serialNumber, stock) = GeneralData();

        Console.WriteLine(" --- Now VRAM capacity");
        int vram = int.Parse(Console.ReadLine());

        
        GraphicCard NewGraphicCard = new GraphicCard (name, serialNumber, price, stock, vram)
        {
            Name = name,
            Price = price,
            SerialNumber = serialNumber,
            Stock = stock,
            Vram = vram

        };

        MyInventory.AddItem(NewGraphicCard);
        Console.WriteLine("\nGraphic Card added!");

        
    }

    static void RAMAdd()
    {
         var (name, price, serialNumber, stock) = GeneralData();

        Console.WriteLine(" --- Now Memory capacity");
        int memory = int.Parse(Console.ReadLine());

        Console.WriteLine(" --- What is the memory type?");

        Console.WriteLine("1. DDR3");
        Console.WriteLine("2. DDR4");
        Console.WriteLine("3. DDR5");

        int MemoryChoice;

        while(!int.TryParse(Console.ReadLine(), out MemoryChoice) || MemoryChoice < 1 || MemoryChoice > 3)
        {
            Console.WriteLine("Invalid Option, Please Select 1, 2 or 3: ");
        }

        MemoryType selectedMemory = MemoryType.DDR4;
        if(MemoryChoice == 1)
        {
            selectedMemory = MemoryType.DDR3;
        }
        else if(MemoryChoice == 2)
        {
            selectedMemory = MemoryType.DDR4;
        }
        else if(MemoryChoice == 3)
        {
            selectedMemory = MemoryType.DDR5;
        }

        RAM NewRam = new RAM (name, serialNumber, price, stock, memory, selectedMemory)
        {
            Name = name,
            Price = price,
            SerialNumber = serialNumber,
            MemoryCapacity = memory

        };

        MyInventory.AddItem(NewRam);
        Console.WriteLine("\nRAM added!");
        
    }

    static void MBAdd()
    {

        var (name, price, serialNumber, stock) = GeneralData();

        Console.WriteLine("Wich socket does it accept?");
        Console.WriteLine("1. AM4");
        Console.WriteLine("2. AM5");
        Console.WriteLine("3. LGA1151");
        Console.WriteLine("4. LGA1200");
        Console.WriteLine("5. LGA1700");

        
        int socketType;

        
        while(!int.TryParse(Console.ReadLine(), out socketType) || socketType < 1 || socketType > 5)
        {
            Console.WriteLine("Invalid Option, Please Select between 1 and 5: ");
        }
        

        SocketType SelectedSocket = SocketType.LGA1151;

        if(socketType == 1)
        {
            SelectedSocket = SocketType.AM4;
        }
        else if(socketType == 2)
        {
            SelectedSocket = SocketType.AM5;
        }
        else if(socketType == 3)
        {
            SelectedSocket = SocketType.LGA1151;
        }
        else if(socketType == 4)
        {
            SelectedSocket = SocketType.LGA1200;
        }
        else if(socketType == 5)
        {
            SelectedSocket = SocketType.LGA1700;
        }

           Console.WriteLine(" --- Which memory Type does it accept?");

        Console.WriteLine("1. DDR3");
        Console.WriteLine("2. DDR4");
        Console.WriteLine("3. DDR5");

        int MemoryChoice;

        while(!int.TryParse(Console.ReadLine(), out MemoryChoice) || MemoryChoice < 1 || MemoryChoice > 3)
        {
            Console.WriteLine("Invalid Option, Please Select 1, 2 or 3: ");
        }

        MemoryType selectedMemory = MemoryType.DDR4;
        if(MemoryChoice == 1)
        {
            selectedMemory = MemoryType.DDR3;
        }
        else if(MemoryChoice == 2)
        {
            selectedMemory = MemoryType.DDR4;
        }
        else if(MemoryChoice == 3)
        {
            selectedMemory = MemoryType.DDR5;
        }

        MotherBoard NewMotherBoard = new MotherBoard (serialNumber, name, price, stock, SelectedSocket, selectedMemory)
        {
            Name = name,
            Price = price,
            SerialNumber = serialNumber,
            Stock = stock,

        };

        MyInventory.AddItem(NewMotherBoard);
        Console.WriteLine("\nMother Board added!");

    }

    static (string name, decimal price, string serialNumber, uint stock) GeneralData()
    {
        
        Console.WriteLine(" --- Write Down the name and model!");
        string name =Console.ReadLine();

        Console.WriteLine(" --- Now give it a price");
        decimal price = decimal.Parse(Console.ReadLine());

        Console.WriteLine(" --- Now Serial Number");
        string serialNumber = Console.ReadLine();

        Console.WriteLine(" --- Now, how much pieces of this component do you have?");
        uint stock = uint.Parse(Console.ReadLine());

        return (name, price, serialNumber, stock);
    }

    static void CreateConfig()
    {
        
        Console.WriteLine("Name your configuration!");
        string name = Console.ReadLine();

        MyServices.SaveConfig(name);


    }

    static void AddComponentToPc()
    {
        Console.WriteLine("\nSelect a configuration to start adding components");

        if (!MyServices.ListConfigs())
        {
            return;
        }
        
        int PcChoice;
        while (!int.TryParse(Console.ReadLine(), out PcChoice))
        {
            Console.WriteLine("That is not a number, try again");
        }
        ConfigPc myPc = MyServices.GetConfig(PcChoice);

        if(myPc == null)
        {
            return;
        }

        bool AddMore = true;
        while(AddMore == true)
        {
            Console.WriteLine($"=== Your are adding components to: {myPc.Name}");
            Console.WriteLine("Select Another Piece:");

            MyInventory.ListItems();

            int itemChoice;
            if(int.TryParse(Console.ReadLine(), out itemChoice))
            {
                try
                {
                Component ChoosenItem = MyInventory.GetComponent(itemChoice);

                myPc.AddComponent(ChoosenItem);      
                }
                catch (ComponentNotFoundException ex)
                {
                    Console.WriteLine($"\nInventory Error: {ex.Message}");
                }
                catch (ComponentException ex)
                {
                    Console.WriteLine($"Compatibility Error: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("\nPlease write a valid number");
            }

            Console.WriteLine("\nDo you want to add another piece? (y/n)");
            string answer = Console.ReadLine().ToLower();

            if(answer != "y")
            {
                AddMore = false;
                Console.WriteLine("\nExiting Configuration...");
            }

        }

    }

    static void ShowPCDetails()
    {
        Console.WriteLine("\nFrom wich configuration you want to see the components?");

        MyServices.ListConfigs();

        int choice;
        while(!int.TryParse(Console.ReadLine(), out choice))
        {
            
            Console.WriteLine("That is not a valid number, try again");
        }
        try
        {
            ConfigPc myPc = MyServices.GetConfig(choice);

             myPc.ListComponents();
        }
        catch(Exception ex)
        {
            Console.WriteLine($"\nSearch Error: {ex.Message}");
        }

    }

    static void RemoveComponentFromConfig()
    {
        Console.WriteLine("\nSelect a configuration to remove components from");

        if (!MyServices.ListConfigs())
        {
            return;
        }

        int PcChoice;
        
        while (!int.TryParse(Console.ReadLine(), out PcChoice))
        {
            Console.WriteLine("That is not a number, try again");
        }

        ConfigPc myPc = MyServices.GetConfig(PcChoice);

        if(myPc == null)
        {
            return;
        }

        bool removeMore = true;

        while(removeMore == true)
        {
            if (!myPc.HasComponents())
            {
                Console.WriteLine("\n This Pc has no components to remove!");
                break;
            }
            Console.WriteLine($"\n=== Removing components from: {myPc.Name} ===");
            Console.WriteLine("Select the number of the piece to remove: ");

            myPc.ListComponents();
            int itemChoice;

            if(int.TryParse(Console.ReadLine(), out itemChoice))
            {
                try
                {
                myPc.RemoveComponent(itemChoice);   
                Console.WriteLine("\nComponent Removed Succesfully");
                }
                catch(ComponentNotFoundException ex)
                {
                    Console.WriteLine($"Removal Error: {ex.Message}");
                }
                catch(ComponentException ex)
                {
                    Console.WriteLine($"\nAction Error: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid option");
            }

            Console.WriteLine("\n Do you want to remove another piece? (y/n)");
            String answer = Console.ReadLine().Trim().ToLower();

            if (answer != "y")
            {
                removeMore = false;
                Console.WriteLine("\nExiting edit mode...");
            }
        }
    }

}