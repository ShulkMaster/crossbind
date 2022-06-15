﻿using CommandLine;
using CrossBind.Commands;
using CrossBind.Compiler;
using CrossBind.Engine;
using CrossBind.Plugin;
using LanguageExt;

namespace CrossBind;

public static class Program
{
    public static int Main(string[] args)
    {
        var commander = Parser.Default;
        return commander.ParseArguments<Project, Compile, int>(args)
            .MapResult<Project, Compile, int>(
                ProjectCommand,
                CompileCommand,
                ErrorHandler
            );
    }

    private static int ProjectCommand(Project command)
    {
        Console.Error.WriteLine("Not implemented");
        return -2;
    }

    private static int CompileCommand(Compile command)
    {
        if (!command.Source.Any())
        {
            Console.Write("No source file directories were given");
            return 0;
        }

        var target = command.Target;
        var engines = new PluginLoader().FindEnginesForTarget(target);
        if (!engines.Any())
        {
            Console.Write("No plugin was found to support the required target: ");
            Console.WriteLine(target);
            return -1;
        }

        IEngine engine;
        if (engines.Count > 1)
        {
            if (string.IsNullOrWhiteSpace(command.PluginId))
            {
                Console.Write("Multiple plugins were found for target: '");
                Console.Write(target);
                Console.WriteLine("' please specify the plugin ID to compile with -p option");
                return -1;
            }

            var engineWithId = engines.FirstOrDefault(e => e.PluginId == command.PluginId);
            if (engineWithId is null)
            {
                Console.Write("No Engine found for target: '");
                Console.Write(target);
                Console.Write("' with ID ");
                Console.Write(command.PluginId);
                return -3;
            }

            engine = engineWithId;
        }
        else
        {
            engine = engines[0];
        }

        Directory.CreateDirectory(command.OutputDir);

        var failedUnits = 0;
        Console.WriteLine(
            "Transpiling COM with Engine: {0} version {1}.{2}.{3}",
            engine.PluginName,
            engine.MajorVersion,
            engine.MinorVersion,
            engine.PathVersion
        );
        foreach (string source in command.Source)
        {
            if (!File.Exists(source))
            {
                Console.WriteLine($"The file {source} could not be found in");
                continue;
            }

            _ = FrontCompiler.CompileUnitFile(source).Match(
                u =>
                {
                    Console.WriteLine("Unit parsed: " + source);
                    string result = engine.CompileUnit(u, false);
                    string filename = source.Split(Path.DirectorySeparatorChar)[^1];
                    filename = Path.ChangeExtension(filename, ".tsx");
                    string filePath = Path.Combine(command.OutputDir, filename);
                    using var file = File.CreateText(filePath);
                    file.WriteLine(result);
                    return Unit.Default;
                },
                e =>
                {
                    failedUnits++;
                    Console.WriteLine(e.Message);
                    return Unit.Default;
                }
            );
        }

        if (failedUnits > 0)
        {
            Console.WriteLine("Total failed units {0}", failedUnits);
        }

        return 0;
    }

    private static int ErrorHandler(IEnumerable<Error> errors)
    {
        foreach (var error in errors)
        {
            Console.WriteLine(error);
        }

        return -1;
    }
}