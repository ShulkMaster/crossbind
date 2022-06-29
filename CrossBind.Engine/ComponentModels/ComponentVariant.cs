using CrossBind.Engine.StyleModel;

namespace CrossBind.Engine.ComponentModels;

public class VariantStyle
{
    public string ValueKey { get; set; } = string.Empty;
    public List<ComponentStyle> VariantStyles { get; init; } = new();
}

public class ComponentVariant
{
    public string Name { get; set; } = string.Empty;
    public List<VariantStyle> Styles { get; init; } = new();

    public IEnumerable<string> GetKeys() => Styles.Select(s => s.ValueKey);
}