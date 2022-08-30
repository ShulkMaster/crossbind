using System.Text.Json.Nodes;

namespace CrossBind.Engine;

/// <include file='Docs/ICrossPlugin.xml' path='docs/class/*'/>
public interface ICrossPlugin
{
    /// <include file='Docs/ICrossPlugin.xml' path='docs/Version/*'/>
    public Version Version { get; }

    /// <include file='Docs/ICrossPlugin.xml' path='docs/Name/*'/>
    public string Name { get; }
    
    /// <include file='Docs/ICrossPlugin.xml' path='docs/PluginId/*'/>
    public string PluginId { get; }
    
    /// <include file='Docs/ICrossPlugin.xml' path='docs/Target/*'/>
    public string Target { get; }

    /// <include file='Docs/ICrossPlugin.xml' path='docs/GetEngineInstance/*'/>
    public IEngine GetEngineInstance(bool production, JsonObject? options);
}