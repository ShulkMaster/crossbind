namespace CrossBind.Engine.Types;

public record StringLiteralType(string Name, string FQDM): TypeModel(Name, FQDM, false)
{
    public string Value { get; init; } = string.Empty;
}