using CrossBind.Engine.BaseModels;
using CrossBind.Engine.Generated;

namespace CrossBind.Engine;

/// <include file='Docs/Api/IEngine.xml' path='docs/class/*'/>
public interface IEngine
{
    /// <include file='Docs/Api/IEngine.xml' path='docs/PluginId/*'/>
    public string PluginId { get; }
    public string PluginName { get; }
    public int MajorVersion { get; }
    public int MinorVersion { get; }
    public int PathVersion { get; }
    public EngineTarget Target { get; }
    
    /// <include file='Docs/Api/IEngine.xml' path='docs/CompileUnitFile/*'/>
    public SourceFile[] CompileUnit(UnitModel unit, bool production);

}

public enum EngineTarget
{
    React,
    Vue,
}