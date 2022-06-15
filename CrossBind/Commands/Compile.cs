using CommandLine;
using CrossBind.Engine;

namespace CrossBind.Commands;

[Verb("compile", HelpText = "Compiles a project or file with the supplied options")]
public class Compile
{
    [Option('o', "output", HelpText = "sets the output of the transpile files", Default = "out")]
    public string OutputDir { get; set; } = "out";
    
    private const string TargetDesc = "Configures the compiler to generate code for the given target using the appropiate pluggin if available";

    [Option('t', "target", HelpText = TargetDesc, Required = true)]
    public EngineTarget Target { get; set; }
     
    private const string SrcDesc = "The entry point of the project to compile or a list of independent source files to transpile";

    [Option('s', "source", HelpText = SrcDesc, Required = true)]
    public IEnumerable<string> Source { get; set; } = Array.Empty<string>();

    private const string PluginDesc = "The id of the plugin vendor to use";
    
    [Option('p', "plugin", HelpText = PluginDesc, Required = false)]
    public string PluginId { get; set; } = string.Empty;

}