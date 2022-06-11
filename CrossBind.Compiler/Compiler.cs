using Antlr4.Runtime;
using CrossBind.Compiler.Plugin;
using CrossBind.Compiler.Visitors;
using CrossBind.Engine;
using CommandLine;
using CrossBind.Compiler.Commands;

namespace CrossBind.Compiler;

public static class Compiler
{
    public static int Main(string[] args)
    {
        var result = CommandLine.Parser.Default.ParseArguments<Project>(args)
            .WithParsed(HandleProject)
            .WithNotParsed(HandleError);
        if (result.Errors.Any())
        {
            return -1;
        }

        var engines = new PluginLoader().FindEnginesForTarget(EngineTarget.React);
        var fileStream = File.OpenRead(Path.Combine("code", "button.hbt"));
        var unitVisitor = new UnitVisitor();
        var charStream = new AntlrInputStream(fileStream);
        var lexer = new HaibtLexer(charStream);
        var stream = new CommonTokenStream(lexer);
        var parse = new HaibtParser(stream);
        var gg = unitVisitor.VisitTranslationUnit(parse.translationUnit());
        fileStream.Close();
        fileStream.Dispose();
        var sources = new List<string>();
        foreach (var engine in engines)
        {
            var res = engine.CompileUnit(gg, false);
            sources.Add(res);
        }

        for (var index = 0; index < sources.Count; index++)
        {
            string source = sources[index];
            using var outputStream = new FileStream($"code{index}.tsx", FileMode.Create);
            var writer = new StreamWriter(outputStream);
            writer.Write(source);
            writer.Flush();
            writer.Close();
            writer.Dispose();
        }

        return 0;
    }

    private static void HandleError(IEnumerable<Error> errors)
    {
        foreach (var error in errors)
        {
            Console.Error.WriteLine(error.Tag.ToString());
        }
    }

    private static void HandleProject(Project project)
    {
        Console.WriteLine(project.Name);
        Console.WriteLine(project.OutputDir);
        foreach (string target in project.Targets)
        {
            Console.WriteLine(target);
        }
    }
}