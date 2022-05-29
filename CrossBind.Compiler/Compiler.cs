using Antlr4.Runtime;
using CrossBind.Compiler.Visitors;

namespace CrossBind.Compiler;

public static class Compiler
{
    private const string _basePath = "code";

    public static int Main(string[] args)
    {
        var fileStream = File.OpenRead(Path.Combine(_basePath, "button.hbt"));
        var unitVisitor = new UnitVisitor();
        var charStream = new AntlrInputStream(fileStream);
        var lexer = new HaibtLexer(charStream);
        var stream = new CommonTokenStream(lexer);
        var parse = new HaibtParser(stream);
        var gg = unitVisitor.VisitTranslationUnit(parse.translationUnit());
        fileStream.Close();
        fileStream.Dispose();
        return 0;
    }
}