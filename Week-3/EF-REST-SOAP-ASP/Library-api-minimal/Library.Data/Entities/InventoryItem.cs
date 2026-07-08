namespace Library.Data.Entities;

public class InventoryItem
{
    
public int Id {get; set;} //FK - 1:1 with product
public int ProductId {get; set;}
public Product Product {get; set;} = default!; // We can have EF give a default value
public int CurrentStock {get; set;} //How many of this things do we 

//Adding a RowVersion property - we will use this in OnModelCreation
//We will use this to track concurrency
public byte[] RowVersion {get; set;} = default!;


}