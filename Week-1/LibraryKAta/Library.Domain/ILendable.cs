namespace LibraryKata.Domain;

//interfaces in C# they are a contract for behaviors, they do not define the implementation of the methods withing
//
public interface ILendable
{
    //Only method signatures, not bodies, not even acces modifiers.
  bool Checkout();
  void Return();  
  
}