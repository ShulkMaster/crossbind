using CrossBind.Engine.BaseModels;

namespace CrossBind.Engine.ComponentModels;

public class ComponentModel : BindModel
{
    [Obsolete("Inheritance capabilities are not discarded, do not use")]
    public readonly Extendable Extends;
    public ComponentBody Body { get; init; } = new ();
    
    public ComponentModel(): base(ModelType.Component)
    {
        Extends = Extendable.Button;
    }
    
    [Obsolete("Use empty constructor")]
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