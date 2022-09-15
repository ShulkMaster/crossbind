using CrossBind.Engine.ComponentModels;

namespace CrossBind.Engine.Markup;

public sealed class ComponentTag : Tag
{
    public readonly ComponentModel Component;

    public ComponentTag(ComponentModel component) : base(component.Name)
    {
        Component = component;
        Attributes = component.Body.Props;
    }
}