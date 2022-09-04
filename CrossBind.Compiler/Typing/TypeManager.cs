using CrossBind.Engine.Types;

namespace CrossBind.Compiler.Typing;

public class TypeManager
{
    private readonly Dictionary<string, TypeModel> _types = new();

    public bool RegisterType(TypeModel model)
    {
        bool exist = _types.ContainsKey(model.FQDN);
        if (exist)
        {
            return false;
        }

        _types.Add(model.FQDN, model);
        
        return true;
    }

    public TypeModel? GetByFQDM(string fqdm)
    {
        _types.TryGetValue(fqdm, out TypeModel? model);
        return model;
    }

    public TypeModel[] GetByName(string name)
    {
       var enumerator = _types
           .Filter(t => t.Value.Name == name)
           .Select(t => t.Value);
       return enumerator.ToArray();
    }
}