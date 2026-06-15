namespace LibraryKata.Domain;

//Library Item will be an abstract class - it cannot be instanciated
//It will still have a constructor - because child classes NEED to be able to call
// their parent's constructor - but we can't call it via new
public abstract class LibraryItem
{
     //Things about a  book we can model
    //because i want to use a no-arg Constructor, its best practice to make my properties nullable
    public string? Title {get; private set;} //autoproperty sintax - no writting getter and setters
    public string? Author {get; private set;}

    //The same way we can have static methods (belong for the class) we can have static properties/members

    private static int _nextId = 1; //By convection, static properties have an underscore

    public int Id { get;} //no setter i dont want someone to reasign this

    //My abstract class DOES have a constructor 
    //So far, we've dealt with public and private access modifiers
    //public: anyone can see/call this
    //Private: only accesible within this class
    //Protected - this class and derived (child) classes only

    protected LibraryItem(string title, string author)
    {
        Id = _nextId++;
        Title = title;
        Author = author;

    }

    //Abstract method - only a signature - no body
    public abstract string Describe();

    //Abstract classes CAN contain concretes implementation - and we can mix our abstract methods to save time later
    //Our child WILL implement Describe() - use that for the ToString();

    public override string ToString() => Describe();

    //concrete methods have a body abstract methods MUST be overriden... virtual methods havce a body and MAY be overriden
    public virtual string ShelfLabel()
    {
        return $"{Id}: {Title}";
    }

}
