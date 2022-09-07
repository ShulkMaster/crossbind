namespace CrossBind.Engine.Types;

public record UnionType(string Name, string FQDM, bool Nullable) : TypeModel(Name, FQDM, Nullable)
{
    public readonly HashSet<TypeModel> TypeModels = new ();
}
