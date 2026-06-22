namespace ComponentStore.Domain;
public class ComponentNotFoundException : Exception
{
    public int Id { get; }

    public ComponentNotFoundException(int id)
        : base($"No component with id {id}")
    {
        Id = id;
    }
}