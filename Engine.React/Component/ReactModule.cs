using CrossBind.Engine.BaseModels;

namespace Engine.React.Component;

public class ReactModule
{
    public readonly string ModuleId;
    
    public SortedSet<ImportModel> Imports { get; } = new();
    
    public List<ReactComponent> Components { get; set; } = new();

    
    
    public ReactModule(string moduleId)
    {
        ModuleId = moduleId;
    }
}