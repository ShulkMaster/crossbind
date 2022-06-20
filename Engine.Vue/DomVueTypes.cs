using CrossBind.Engine.ComponentModels;

namespace Engine.Vue;

public static class DomVueTypes
{
    public static string GetComponentTag(Extendable extend)
    {
        return extend switch
        {
            Extendable.Button => "button",
            Extendable.Select => "select",
            Extendable.TextBox => "input",
            Extendable.Component => string.Empty,
            _ => throw new ArgumentOutOfRangeException(nameof(extend), extend, null)
        };
    }
}


