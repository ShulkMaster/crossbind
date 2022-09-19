using CrossBind.Engine.BaseModels;
using CrossBind.Engine.ComponentModels;

namespace CrossBind.Engine.Markup;

public sealed class ComponentTag : Tag
{
    public readonly ComponentModel Component;
    public List<AttributeModel> Attribs { get; init; } = new();

    public ComponentTag(ComponentModel component) : base(component.Name)
    {
        Component = component;
    }
}