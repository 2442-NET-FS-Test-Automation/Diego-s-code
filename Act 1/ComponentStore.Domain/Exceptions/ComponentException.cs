namespace ComponentStore.Domain;

public class ComponentException :  Exception
{
    public ComponentException(string message) : base(message){ }
}