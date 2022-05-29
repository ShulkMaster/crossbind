using CrossBind.Model;

namespace CrossBind.Engine;

public interface IEngine
{
    public string PluginName { get; }
    public int MajorVersion { get; }
    public int MinorVersion { get; }
    public int PathVersion { get; }
    public EngineTarget Target { get; }

    public string CompileUnit(UnitModel model, bool production);

}

public enum EngineTarget
{
    React,
    vue,
}