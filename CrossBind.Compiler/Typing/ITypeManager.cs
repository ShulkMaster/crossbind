using CrossBind.Engine.Types;

namespace CrossBind.Compiler.Typing;

public interface ITypeManager
{
    bool RegisterType(TypeModel model);
    TypeModel? GetByFQDM(string fqdm);
    TypeModel[] GetByName(string name);
}