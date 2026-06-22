namespace ComponentStore.Domain;


public class BuildServices{
private Dictionary <string, ConfigPc> _builds = new Dictionary<string, ConfigPc>();

public void SaveConfig(string BuildName)
{
    ConfigPc NewPc = new ConfigPc(BuildName); 
    _builds.Add(BuildName, NewPc);

    Console.WriteLine(" === Your Configuration was created! ===");

}

public bool ListConfigs()
    {
        if(_builds.Count == 0)
        {
            Console.WriteLine("There are no configurations. Create One!");
            return false;
        }

        Console.WriteLine("=== Your Configurations ===");
        int i = 1;
        
        foreach(var kvp in _builds){
            Console.WriteLine($"{i}. {kvp.Key}");
            i++;
        } 

        return true;
    }



public ConfigPc GetConfig(int choice)
{

    if( choice < 1 || choice > _builds.Count)
        {
            throw new Exception($"This number doesn't exist: {choice}");
        }

        string targetName = "";
        int currentIndex = 1;

        foreach(var key in _builds.Keys)
        {
            if(currentIndex == choice)
            {
                targetName = key;
                break;
            }
            currentIndex++;
        }

    if(_builds.TryGetValue(targetName, out ConfigPc foundPc))
        {
            return foundPc;
        }
            throw new Exception($"The configuration {targetName} could not be loaded");
}


}