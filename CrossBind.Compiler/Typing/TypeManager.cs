using CrossBind.Engine.Types;

namespace CrossBind.Compiler.Typing;

public class TypeManager : ITypeManager
{
    private readonly Dictionary<string, TypeModel> _types = new();

    public TypeManager()
    {
        Primitive primitive = Primitive.Number(false);
        _types.Add(primitive.FQDN, primitive);
        
        primitive = Primitive.String(false);
        _types.Add(primitive.FQDN, primitive);
        
        primitive = Primitive.Bool(false);
        _types.Add(primitive.FQDN, primitive);
    }

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