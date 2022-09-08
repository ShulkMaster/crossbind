using CrossBind.Engine.BaseModels;
using CrossBind.Engine.StyleModel;

namespace CrossBind.Engine.ComponentModels;

public class ComponentBody
{
    public List<ComponentStyle> BaseStyles { get; init; } = new();
    public List<ComponentVariant> Variants { get; init; } = new();
    public List<PropModel> Props { get; init; } = new();
}