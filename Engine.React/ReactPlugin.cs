using System.Text.Json.Nodes;
using CrossBind.Engine;

namespace Engine.React;

public class ReactPlugin: ICrossPlugin
{
    public Version Version => new Version(0,1,0);
    public string Name => "React Plugin Official";
    public string PluginId => "REACT_TS_CROSSBIND_OFFICIAL";
    public string Target => "REACT";

    public IEngine GetEngineInstance(bool production, JsonObject? options)
    {
        return new ReactEngine();
    }
}