using CrossBind.Engine;
using CrossBind.Engine.BaseModels;

namespace Engine.React;

public class ReactEngine : IEngine
{
    public string PluginName => "React Engine Official";
    public int MajorVersion => 0;
    public int MinorVersion => 1;
    public int PathVersion => 0;
    public EngineTarget Target => EngineTarget.React;

    public string CompileUnit(UnitModel model, bool production)
    {
        var importStatements = model.Modules
            .Select(import => $"import {{{import.Simbols.Aggregate((symbol1, symbol2 ) => $"{symbol1}, {symbol2}" )}}} from {import.Path};");
        return importStatements.Aggregate((s, index) => s + "\n" + index);
    }
}