//Lets actually start modeling stuff

using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace LibraryKata.Domain;

public class OldBook
{
    //Things about a  book we can model
    //because i want to use a no-arg Constructor, its best practice to make my properties nullable
    public string? Title {get; private set;} //autoproperty sintax - no writting getter and setters
    public string? Author {get; private set;}
    public int? CopiesAvaliable{get; private set;}

    //The same way we can have static methods (belong for the class) we can have static properties/members

    private static int _nextId = 1; //By convection, static properties have an underscore

    public int Id { get;} //no setter i dont want someone to reasign this

    //every class has a very specific method within it. The constructor - you can have as many as you need/want
    public OldBook(string title, string author, int copiesAvaliable)
    {
        Id = _nextId++; //get the value of _nextId, assign it, increment it   
        Title = title;
        Author = author;
        CopiesAvaliable = copiesAvaliable;

        _nextId++;

    }

    //overloading class
    public OldBook() { }

    //our first instance method - no "static" keyword, just an acces modifier + return type + any arguments if any
    public bool Checkout()
    {
        if(CopiesAvaliable == 0)
        //Attempt to checkout a book - if copies is already 0, return false
            return false;

        //otherwise, we pass over the above code block
        CopiesAvaliable--;
        return true;
        
    }
        //Providing for return behavior
          public void Return() => CopiesAvaliable++;

    //Overriding a toString
    public override string ToString()
    {
        //we can use the base keyword to refer to the parent class we are working in return base.ToString();
        return $"{Title} by {Author}: {CopiesAvaliable} avaliable for checkout";
    }

}