using CrossBind.Engine.BaseModels;

namespace CrossBind.Engine.ComponentModels;

public class ComponentModel : BindModel
{
    public readonly Extendable Extends;
    public ComponentBody Body { get; } = new ();
    
    public ComponentModel(Extendable extends): base(ModelType.Component)
    {
        Extends = extends;
    }

}

public enum Extendable {
    Button,
    Select,
    TextBox,
    Component
}