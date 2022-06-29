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
        bool isPath = Path.IsPathRooted(filePath);

        var listener = new HaibtLexerErrorListener();
        var unitVisitor = new UnitVisitor
        {
            FilePath = filePath,
        };
        IDisposable? disposable = null;
        AntlrInputStream charStream;
        if (isPath)
        {
            FileStream fileStream = File.OpenRead(filePath);
            charStream = new AntlrInputStream(fileStream);
            disposable = fileStream;
        }
        else
        {
            charStream = new AntlrInputStream(filePath);
        }

        var lexer = new HaibtLexer(charStream);
        lexer.AddErrorListener(listener);
        var stream = new CommonTokenStream(lexer);
        var parse = new HaibtParser(stream);
        parse.AddErrorListener(new ErrorLister());
        HaibtParser.TranslationUnitContext? parseTree = parse.translationUnit();
        disposable?.Dispose();
        if (parse.NumberOfSyntaxErrors > 0 || listener.GetErrors().Any())
        {
            foreach (string error in listener.GetErrors())
            {
                Console.WriteLine(error);
            }

            return new Result<UnitModel>(new InvalidDataException());
        }
        UnitModel gg = unitVisitor.VisitTranslationUnit(parseTree);
        return gg;
    }
}