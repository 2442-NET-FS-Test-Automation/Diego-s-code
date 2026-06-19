namespace ComponentStore.Domain;

interface IRepositoryComponent
{
    void Add(Component component);
    List<Component>GetAll(); 
    Component GetById(int id);
    bool Remove(int id);
}
