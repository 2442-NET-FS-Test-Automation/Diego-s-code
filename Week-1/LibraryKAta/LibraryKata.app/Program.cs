using System.Data;
using System.Runtime.CompilerServices;

//if i have code from another spacename i want to use here - I use a using statement
using LibraryKata.Domain;

namespace LibraryKata.app; //A namespace is like a bucket o logical containter for different related code files.

public class Program
{

    public static void Main()
    {

        //When i call dotnet run, it finds Main and beggins code execution at the firs line of the Main method 
        DataTypesAndOperators();
        ClassExample();
        OopDemo();
        CollectionDemo();
        
    }

    private static void DataTypesAndOperators()
    {

        //C# is a strongly typed lenguaje 

        Console.WriteLine("Data types and operators");
        int copies = 3; //Whole numbers
        double lateFee = 1; //Decimal numbres
        bool isMember = true; //True or false
        char shelf = 'A'; //Single character
        string title = "Clean code"; //text

        //operators

    string user = "Diego"; // Single = is the assignament operaton
    int total = copies * 2; //Examples of an arithmetic operators like + - * / 
    bool isEnough = total > 4;  //Comparasion

    bool exactlySix = total == 6; //Equality 
    bool lendable = isMember && isEnough; //Logical operators
    // && - and, || - or, != -reverses the condition that follows,

    Console.WriteLine(title + " has been checked out by " + user);
    Console.WriteLine($"{title} on shelf {shelf}: {copies} copies, fee {lateFee}");

    total += 1;

    }

    private static void ControlFlow()
    {
        
        Console.WriteLine("\n-- Control FLow --");

        int CopiesAvaliable = 0;
        bool isMember = true;

        if(CopiesAvaliable > 1){
            Console.WriteLine("Many avaliable for checkout!");
        }
        else if(CopiesAvaliable == 1){
            Console.WriteLine("Last copy!");
        }
        else{ 
            Console.WriteLine("Out of stok");
        }

    //Switch
    string genre = "Mystery";


    //Clasic switch - notice c# cores about intent alot! No fall through like in other lenguages
        switch (genre)
        {
            case "Mystery":
                Console.WriteLine("Check section A");
                break;
            case "Science-Fiction":
                Console.WriteLine("Check Section F");
                break;
            default:
                Console.WriteLine("Uh oh");
                break;
        }

        // New in .Net 8 Switch expresions 

        string section = genre switch
        {
            //this is my expresion body

        "Mystery" => "Section A",
        "Science" => "Section F",
        _ => "uh oh" //default

        };

        Console.WriteLine(section);    

    }

    private static void Loops()
    {
        //C# provides for loops as well, some as Java and any other Lenguage
        //For, whilw do-while, etc
        for(int day = 1; day <=3; day++)
        {
            Console.WriteLine($"Reminder day {day}: fee so far ");
        }

        int onShelf = 3;
        while (onShelf > 0)
            Console.WriteLine($"(onShlef) copies on the shelf!");
            onShelf--;

            Console.WriteLine("No copies on shelf!");

            string myString = "dog";

            myString = "cat";
    }

    private static decimal CalculateLateFee (int daysLate) => daysLate * 2;

    private static void ArraysWork()
    {
        
        //C# provides for arrays as well as lists and other collection
        string[] books = {"Dune", "Harry potter", "Percy Jackson", "Lord of the rings"};

        Console.WriteLine(books[2]); //I can acces individual elements
        
        //C# Allows for for-each Loops

        foreach(string book in books)
        {
            Console.WriteLine(book);
        }
    }

    public static void ClassExample()
    {
        Console.WriteLine("Calling our domain Book class");

        //instanciating my first book calling the constructor via "new" keyword
        Book dune = new Book("Dune", "Frank Herbert", 3 );
        Book littlePrince = new Book("Little prince", "Antonie de Saint-Exupery", 0);

        // If i want to print book info, i can just pass the book available
        //It calls the Tostring() for me. the next two lines do the same thing
        Console.WriteLine(dune);
        Console.WriteLine(littlePrince.ToString());

        Console.WriteLine($"Checking out Dune: {dune.Checkout()}"); //True
        Console.WriteLine($"Checking out The little prince: {littlePrince.Checkout()}"); //false
    }

    public static void OopDemo()
    {
        Console.WriteLine("\n\n OOP Demo Stuff --");

    //Leveageing polimorphism - Books, Reference, Magazines - all are libraryItems.
        LibraryItem[] catalog =
        {
          new Book("Dune", "Frank Herbert", 2),
          new ReferenceBook ("C# Lenguage Standars", "Microsoft", "Technology"),
          new Magazine ("Sports", "Francisco", 5, "Conde Naste")

        };

        foreach(LibraryItem item in catalog)
        {
            Console.WriteLine(item.Describe());
        }

        //We can even use interface as reference types
        foreach(LibraryItem item in catalog)
        {
            if (item is ILendable lendable)
            {
                Console.WriteLine($"{item.Title}: checkout -> {lendable.Checkout()}");
            }
            else
            {
                Console.WriteLine($"{item.Title} is reference only.");
            }
        }

        // override vs normal behavior
        Magazine wired = new Magazine("Wired", "Luis", 3, "Conde Nest");
        LibraryItem baseMag = wired;

        Console.WriteLine("-- Override vs new on the same object, different ref type");
        Console.WriteLine($"Magazine reference -> {wired.Describe()}");
        Console.WriteLine($"LibraryItem reference -> {baseMag.Describe}");
    }

    //Collection Demo stuff

    private static void CollectionDemo()
    {
        Console.WriteLine("-- COLLECTION DEMO STUFF --");

        //creating a catalog object becaue this is backed by a list, it grows and shrinks for us
        Catalog Catalog = new();

        //I could create my objects
        Book dune = new Book("Dune", "Frank Herbert", 3);

        //then add items
        Catalog._items.Add(dune); 

        //I can also just call a constructor inside the Add() method call
        //Methods having their arguments satisfied by the return of other methodsis a common pattern
        //and sometimes you'll get like 4-5 callbakcs deep in tools like ASP.NET

        Catalog._items.Add(new ReferenceBook("C# Lenguage specs", "Microsoft", "Tech"));
        
        Catalog._items.Add(new Magazine("NatGeo", "Charlie", 5, "Conde Naste"));

        Console.WriteLine($"Catalog holds {Catalog._items.Count}, first is {Catalog._items[0].Title}");

        //Enum + struct use
        ItemKind kind = ItemKind.Magazine; //Example of selecting an enum value

        ShelfLocation location = new ShelfLocation(3, 12); //struct - lookjs a lot like a class, but in a VALUE type

        Console.WriteLine($"{kind} sits at {location}");

        Book duneCopy = dune; //Copies the reference
        //lets say i modify duneCopy, what happens to the data in dune?
        //all we copied was the pointer - these two things are not independent 

        ShelfLocation location2 = location; //copies the data/fields
        //these are not limited in the same way, I can edit the data in one without touching the other

        //Generics: our own Shelf<T> that can hold anything - though technically  all the collections
        //we used thusfar have been generic classes themselves

        Shelf<LibraryItem> shelf = new Shelf<LibraryItem>(10);
        Shelf<int> intShelf = new Shelf<int>(200);

        shelf.TryAdd(Catalog._items[0]);
        shelf.TryAdd(Catalog._items[1]);

        Console.WriteLine($"Trying to add a third thing in our catalog: {shelf.TryAdd(Catalog._items[2])}");
        

    }

}