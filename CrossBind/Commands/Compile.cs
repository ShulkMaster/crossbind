using CommandLine;
using MediatR;

namespace CrossBind.Commands;

[Verb("compile", isDefault: true, HelpText = "Compiles a project or file with the supplied options")]
public class Compile: IRequest<int>
{
    [Option('o', "output", HelpText = "sets the output of the transpile files", Default = "out")]
    public string OutputDir { get; set; } = "out";

    private const string SrcDesc = "The entry point of the project to compile or a list of independent source files to transpile";

    [Option('s', "source", HelpText = SrcDesc, Required = true)]
    public string Source { get; set; } = string.Empty;

    private const string PluginDesc = "The id of the plugin vendor to use";
    
    [Option('p', "plugin", HelpText = PluginDesc, Required = true)]
    public string PluginId { get; set; } = string.Empty;
    
    [Option('w', "watch", Required = false, Default = false)]
    public bool Watch { get; set; }

    private const string CfgDesc = "The file with the configurations properties for the compilation";
        
    [Option('c', "config", HelpText = CfgDesc, Required = false, Default = "crossbind.json")]
    public string Config { get; set; } = "crossbind.json";

}