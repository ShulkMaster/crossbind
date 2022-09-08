using CrossBind.Engine.BaseModels;
using CrossBind.Engine.ComponentModels;

namespace Engine.React.Component;

public class ReactComponent
{
    public string Name { get; init; } = string.Empty;
    public List<PropModel> Properties { get; init; } = new();
    public ComponentType ComponentType { get; init; } = ComponentType.Functional;
    public ComponentModel Model { get; init; } = null!;
}