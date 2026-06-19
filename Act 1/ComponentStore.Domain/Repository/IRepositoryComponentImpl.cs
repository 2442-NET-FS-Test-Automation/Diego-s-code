using Serilog;

namespace ComponentStore.Domain;

public class IRepositoryComponentImplementation : IRepositoryComponent
{
    private readonly Dictionary<int, Component> _items = new();
    public void Add(Component component)
    {
        _items.Add(component.Id, component);   
        Log.Information($"Added {component.Name} - id: {component.Id}");
    }
    public List<Component>GetAll() => _items.Values.ToList();
    public Component GetById(int id)
    {
        if (_items.TryGetValue(id, out Component? item))
        {
            return item;
        }
        Log.Warning("Lookup failed for id {Id}", id);
        throw new Exception();
        //throw new ItemNotFoundException(id);
    }
    public bool Remove(int id)
    {
        if(_items.Remove(id))
        {
            Log.Information("Removed item with id {Id}", id);
            return true;
        }
        Log.Information("Removal failed for item with id {Id}", id);
        return false;
    }
}
