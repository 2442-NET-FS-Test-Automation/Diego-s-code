namespace LibraryKata.Domain;

public class Book : LibraryItem, ILendable
{
    public int CopiesAvaliable {get; private set;}

    //Child class construtors look a little different
    //We take in all our argouments for the parent + child, then call base with a colon.
    public Book(string title, string author, int copiesavaliable) : base(title, author)
    {
        CopiesAvaliable = copiesavaliable;
    }

    //because we have an abstract method in the parent, we MUST override it or we can't compile
    public override string Describe()
    {
        return $"{Id}: {Title} by {Author} has {CopiesAvaliable} copies avaliable for checkout";
    }

    //Methods below pasted from OldBook.cs
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

}