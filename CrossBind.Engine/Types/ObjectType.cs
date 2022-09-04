namespace CrossBind.Engine.Types;

public record ObjectType(string Name, string FQDM): TypeModel(Name, FQDM)
{
    private readonly Dictionary<string, TypeModel> _dic = new();

    public IEnumerable<TypeModel> Properties => _dic.Values;

    public IEnumerable<string> GetKeys() => _dic.Keys;

    public TypeModel? GetModel(string name)
    {
        _dic.TryGetValue(name, out TypeModel? model);
        return model;
    }
}