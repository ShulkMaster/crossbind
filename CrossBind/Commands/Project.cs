using CommandLine;
using MediatR;

namespace CrossBind.Commands;

[Verb("project", HelpText = "Creates a new project with the define options")]
public class Project: IRequest<int>
{
    [Option('n', "name", HelpText = "sets the project name")]
    public string Name { get; init; } = string.Empty;
    
    [Option('o', "output", HelpText = "sets the output directory of the project template")]
    public string OutputDir { get; init; } = ".";
}