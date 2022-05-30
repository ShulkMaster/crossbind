using CrossBind.Engine.BaseModels;

namespace CrossBind.Engine;

public interface IEngine
{
    public string PluginName { get; }
    public int MajorVersion { get; }
    public int MinorVersion { get; }
    public int PathVersion { get; }
    public EngineTarget Target { get; }

    public string CompileUnit(UnitModel unit, bool production);

}

public enum EngineTarget
{
    React,
    vue,
}