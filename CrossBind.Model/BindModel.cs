namespace CrossBind.Model;

public abstract class BindModel
{
    public readonly ModelType Type; 
    public string Name { get; set; } = string.Empty;
    public string ModuleId { get; set; } = string.Empty;

    protected BindModel(ModelType type)
    {
        Type = type;
    }
}

public enum ModelType
{
    Component,
    ShareLib,
    Asset,
} 