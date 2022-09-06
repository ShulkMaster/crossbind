using CrossBind.Engine.StyleModel;

namespace CrossBind.Engine.ComponentModels;

public class VariantStyle
{
    public string ValueKey { get; set; } = string.Empty;
    public bool Default { get; set; }
    public List<ComponentStyle> VariantStyles { get; init; } = new();
}

public class ComponentVariant
{
    public string Name { get; set; } = string.Empty;
    public List<VariantStyle> Styles { get; init; } = new();
    // hoover, active, disabled etc.
    public string Action { get; set; } = string.Empty;
    public string? DefaultName { get; init; }
    public IEnumerable<string> GetKeys() => Styles.Select(s => s.ValueKey);
}