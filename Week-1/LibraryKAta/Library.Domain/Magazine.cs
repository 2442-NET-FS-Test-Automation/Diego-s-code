namespace LibraryKata.Domain;

public class Magazine: LibraryItem, ILendable
{
    public int CirculationCopies{get; private set;}
    public string Publisher {get; private set;}

    public Magazine(string title, string author, int circulationCopies, string publisher) : base (title, author)
    {
        CirculationCopies = circulationCopies;  
        Publisher = publisher;      
    }

    public override string Describe()
    {
        return $"{Title} magazine, published by {Publisher}";
    }

    //Provide implementations via new instead of override - has implications fot Later
    //This is technically Method Hiding - depends on the reference type
    //Calling this method in an object instantiated like this:
    //LibraryItem sportsIllustrated = new MAgazine(...); - calls LibraryItem's ShelfLabel
    //This is not most likely you want.
    //new vs override - very different behavior

    public new string ShelfLabel()
    {
        return $"MAG-{Id} {Title}";
    }

         public bool Checkout()
    {
        if(CirculationCopies == 0)
        //Attempt to checkout a book - if copies is already 0, return false
            return false;

        //otherwise, we pass over the above code block
        CirculationCopies--;
        return true;
    
    }

    public void Return() => CirculationCopies++;
}