using Antlr4.Runtime;
using CrossBind.Compiler.Plugin;
using CrossBind.Compiler.Visitors;
using CrossBind.Engine;

namespace CrossBind.Compiler;

public static class Compiler
{
    private const string _basePath = "code";

    public static int Main(string[] args)
    {
        var engines = new PluginLoader().FindEnginesForTarget(EngineTarget.React);
        var fileStream = File.OpenRead(Path.Combine(_basePath, "button.hbt"));
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
            using var outputStream = new FileStream($"code{index}.ts", FileMode.Create);
            var writer = new StreamWriter(outputStream);
            writer.Write(source);
            writer.Flush();
            writer.Close();
            writer.Dispose();
        }

        return 0;
    }
}