namespace LibraryKata.Domain;


//Structs are for small bundles of data with no identity.
//They look kinf of like classes but they are VALUE types
//meaning - two structs of the same type with the same data are identical
//If i compare those two structures with .equals() I get tru
public readonly struct ShelfLocation
{
    public int Aisle {get; }
    public int Shelf {get; }
    public ShelfLocation (int aisle, int shelf)
    {
        Aisle = aisle;
        Shelf = shelf;
    }

    public override string ToString()
    {
        return $"Aisle {Aisle}, Shelf {Shelf}";
    }

}