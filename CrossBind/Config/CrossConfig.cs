using System.Text.Json.Nodes;

namespace CrossBind.Config;

public class CrossConfig
{
    public string OutDir { get; set; } = string.Empty;

    public JsonObject? Options { get; set; }
}