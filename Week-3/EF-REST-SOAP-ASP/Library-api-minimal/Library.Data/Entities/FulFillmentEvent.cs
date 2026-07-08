using System.Net.NetworkInformation;

namespace Library.Data.Entities;

public class FulfillmentEvent
{
    public int Id {get; set;}
    public int OrderId{get; set;}

    // = default! is something we're doing fot EF Core. If we ere to make this nullable we'd
    //Satisfy the compiler - but what if I DON'T want the database column to allow a null?
    // =default! Lets me shove some default value (varies per type) into the property on creation
    public string Type {get; set;} = default!;
    public DateTime FulfilledAtUc {get; set;} = DateTime.UtcNow;

}