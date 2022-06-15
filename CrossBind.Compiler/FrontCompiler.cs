using Antlr4.Runtime;
using CrossBind.Compiler.Error;
using CrossBind.Compiler.Visitors;
using CrossBind.Engine.BaseModels;
using LanguageExt.Common;

namespace CrossBind.Compiler;

public static class FrontCompiler
{
    public static Result<UnitModel> CompileUnitFile(string filePath)
    {
        var fileStream = File.OpenRead(filePath);
        var listener = new HaibtLexerErrorListener();
        var unitVisitor = new UnitVisitor()
        {
            FilePath = filePath,
        };
        var charStream = new AntlrInputStream(fileStream);
        var lexer = new HaibtLexer(charStream);
        lexer.AddErrorListener(listener);
        var stream = new CommonTokenStream(lexer);
        var parse = new HaibtParser(stream);
        parse.AddErrorListener(new ErrorLister());
        var parseTree = parse.translationUnit();
        if (parse.NumberOfSyntaxErrors > 0 || listener.GetErrors().Any())
        {
            foreach (string error in listener.GetErrors())
            {
                Console.WriteLine(error);
            }
            return new Result<UnitModel>(new InvalidDataException());
        }

        var gg = unitVisitor.VisitTranslationUnit(parseTree);
        fileStream.Close();
        fileStream.Dispose();

        return gg;
    }
}