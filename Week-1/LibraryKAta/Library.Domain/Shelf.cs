//For demo state, lets write a generic
//I want to create a shelf, and a shlef can hold anything, 
//I dont want to be limited to LibraryItems, I can put like computer hardware or supplies on the shelf

namespace LibraryKata.Domain;


//T is the standar placeholder for... "same type" that we will determine later
//You will see it all over the place in documentation and code examples
public class Shelf<T>
{
        private readonly T[] _slots;
        private int used; //as things added to my array, the shelf object tracks how 
        //Slots of the shelf are being used internally here.
        public Shelf(int capacity)
            {
                 _slots = new T[capacity];
            }

        //Exposing some array properties as needed
        public int Capacity => _slots.Length;
        public int Count => used; //Exposing that use as apublic property

        //Method to add items to our shelf
        public bool TryAdd(T item)
    {
        
        if( used == _slots.Length)
        {
            return false;
        }

        //If the shelf isnt full then...
        //acces the _slots array's index of the current used + 1
        //increment used 
        //Set that index equal to the incoming item 
        _slots[used++] = item;
        return true;
    }

    //Method to allow index acces
    public T Get (int index)
    {
        return _slots[index];
    }
    
}