using Antlr4.Runtime;
using CrossBind.Compiler.Error;
using CrossBind.Compiler.Visitors;
using CrossBind.Engine.BaseModels;
using CrossBind.Engine.ComponentModels;

namespace CrossBind.Compiler;

public static class Compiler
{
    public static UnitModel CompileUnitFile(string filePath)
    {
        var fileStream = File.OpenRead(filePath);
        var unitVisitor = new UnitVisitor();
        var charStream = new AntlrInputStream(fileStream);
        var lexer = new HaibtLexer(charStream);
        lexer.AddErrorListener(new ConsoleErrorListener<int>());
        var stream = new CommonTokenStream(lexer);
        var parse = new HaibtParser(stream);
        parse.AddErrorListener(new ErrorLister());
        if (parse.NumberOfSyntaxErrors > 0)
        {
            Console.Out.WriteLine($"Se detectaros {parse.NumberOfSyntaxErrors} errores");
            return new UnitModel("", filePath, Array.Empty<ImportModel>(), Array.Empty<ComponentModel>());
        }

        var gg = unitVisitor.VisitTranslationUnit(parse.translationUnit());
        fileStream.Close();
        fileStream.Dispose();

        return new UnitModel("", filePath, Array.Empty<ImportModel>(), Array.Empty<ComponentModel>());
    }
}