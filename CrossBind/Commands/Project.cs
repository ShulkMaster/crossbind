using CommandLine;

namespace CrossBind.Commands;

[Verb("project", HelpText = "Creates a new project with the define options")]
public class Project
{
    private const string TargetDesc = "Configures the project to generate code for the given target";
    
    [Option('n', "name", HelpText = "sets the project name")]
    public string Name { get; init; } = string.Empty;
    
    [Option('o', "name", HelpText = "sets the output directory of the project template")]
    public string OutputDir { get; init; } = ".";
    
    [Option('t', "target", HelpText = TargetDesc)]
    public IEnumerable<string> Targets { get; init; } = Array.Empty<string>();
}