using Library.Data.Entities;

namespace Library.Data;

public interface IInventoryRepository
{
    //For now we'll leave this blank
    public Task<IReadOnlyList<InventoryItem>> GetAllAsync();
    public Task<InventoryItem?> GetInventoryItemBySkuAsync(string sku);
    public Task<InventoryItem> AddInventoryItemAsync(string sku, string name, decimal price, int quantity);
    public Task<bool> RemoveSkuAsync(string sku);

}