using CrossBind.Engine;
using CrossBind.Model;

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
        return "export {}";
    }
}