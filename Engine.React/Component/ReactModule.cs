using CrossBind.Engine.BaseModels;
using Engine.React.Import;

namespace Engine.React.Component;

public class ReactModule
{
    public readonly string ModuleId;
    
    public SortedSet<NamedImport> NamedImports { get; } = new();
    public SortedSet<StyleImport> StyleImports { get; } = new();
    public SortedSet<DefaultImport> DefaultImports { get; } = new();
    
    public List<ReactComponent> Components { get; set; } = new();

    
    
    public ReactModule(string moduleId)
    {
        ModuleId = moduleId;
    }


    public void ResolveImports(UnitModel unit)
    {
        DefaultImports.Add(new DefaultImport("react")
        {
            ModuleName = "React"
        });

        foreach (ImportModel importModel in unit.Modules)
        {
            var module = new NamedImport(importModel.Path);
            foreach (string symbol in importModel.Symbols)
            {
                module.Symbols.Add(symbol);
            }
            NamedImports.Add(module);
        }
    }
}