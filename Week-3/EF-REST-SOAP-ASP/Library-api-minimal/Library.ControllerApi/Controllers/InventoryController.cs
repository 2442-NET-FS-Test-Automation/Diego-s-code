using Library.Data;
using Microsoft.AspNetCore.Mvc;

[ApiController] //This anotation tells ASP.NET to map this controller during app.MapControllers()
[Route("api/controller")] //Pretty sure this will be localhost:5051/api/Inventory as the route base
public class InventoryController : ControllerBase
{
    
    //This will be removed tomorrow for sure
    private readonly IInventoryRepository _repo; //boo

    public InventoryController(IInventoryRepository repo)
    {
        _repo = repo;
    }

    //Lets write our first GET endpoint 
    [HttpGet] //IActionResults just represents possible HTTP response actions
    public async Task<IActionResult<InvetoryReturnDTO>> Get()
    {
        //As is this creates an infinite loop when we try ti serialize to Json
        //return Ok(await _repo.GetAllAsync());

        //The fix is using a DTO - Data Transfer Object. In general it is a bad practice
        //To send models as returns (or take them as arguments) to/from controller methods
        //Models are for your API, not the front end

        var items = await _repo.GetAllAsync(); //get All items

        //This is what we will send back once we populate it
        EntireInventoryDTO response = new();

        //now we need to map those DTOs
        foreach (var item in items)
        {
            //Creating an inventoruReturnDTO
            InventoryReturnDTO i = new InventoryReturnDTO
            {
                Name = item.Product.Name,
                Sku = item.Product.Sku,
                CurrentStock = item.CurrentStock
            };

            //To then populate the EntireInventoryDTO
            response.EntireInventory.Add(i);

        }

        //Returning the EntireInventoryDTO
        return Ok(response);
    }

    //Localhost:5137/api/Inventory/{sku} - sku is passed in by the user
    [HttpGet("{sku}")] //I can parameterize the route itself
    public async Task<ActionResult<InventoryReturnDTO>> GetBySku (string sku)
    {
        var item = await _repo.GetInventoryItemBySkuAsyc(sku);

        var response = new InentoryReturnDTO
        {
            Name = item.Product.Name,
            sku = item.Product.Sku,
            CurrentStock = item.CurrentStock
        };

        //Then we check what to return based on item beign null or not
        if(item is null)
            return NotFound();
        else
            return Ok(response); //200 - founf something - sent back to front end
    }
    
}