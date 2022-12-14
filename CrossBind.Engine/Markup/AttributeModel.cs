using CrossBind.Engine.Types;

namespace CrossBind.Engine.Markup;

public abstract class AttributeModel
{
    public string Name { get; }
    public TypeModel Type { get; }
    
    protected AttributeModel(string name, TypeModel type)
    {
        Name = name;
        Type = type;
    }
}

public sealed class ConstAttributeModel : AttributeModel
{
    public string ConsValue { get; init; } = string.Empty;
    
    public ConstAttributeModel(string name, TypeModel type): base(name, type)
    {
    }
}

public sealed class AssignAttributeModel : AttributeModel
{
    public string Identifier { get; init; } = string.Empty;
    public bool Bind { get; set; }
    
    public AssignAttributeModel(string name, TypeModel type): base(name, type)
    {
    }

}