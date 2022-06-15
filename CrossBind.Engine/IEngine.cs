using CrossBind.Engine.BaseModels;
using CrossBind.Engine.Generated;

namespace CrossBind.Engine;

public interface IEngine
{
    public string PluginId { get; }
    public string PluginName { get; }
    public int MajorVersion { get; }
    public int MinorVersion { get; }
    public int PathVersion { get; }
    public EngineTarget Target { get; }

    public SourceFile[] CompileUnit(UnitModel unit, bool production);

}

public enum EngineTarget
{
    React,
    Vue,
}