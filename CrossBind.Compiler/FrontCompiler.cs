using Antlr4.Runtime;
using CrossBind.Compiler.Error;
using CrossBind.Parser.Implementation;
using CrossBind.Compiler.Visitors;
using CrossBind.Engine.BaseModels;
using LanguageExt.Common;

namespace CrossBind.Compiler;

public static class FrontCompiler
{
    public static Result<UnitModel> CompileUnitFile(string file)
    {
        FileStream fileStream = File.OpenRead(file);
        var charStream = new AntlrInputStream(fileStream);
        var result = Compile(file, charStream);
        fileStream.Dispose();
        return result;
    }

    public static Result<UnitModel> CompileUnitSource(string source)
    {
        var charStream = new AntlrInputStream(source);
        return Compile("default.hbt", charStream);
    }
    
    private static Result<UnitModel> Compile(string filePath, AntlrInputStream ss)
    {
        var listener = new HaibtLexerErrorListener();
        var unitVisitor = new UnitVisitor
        {
            FilePath = filePath,
        };
        IDisposable? disposable = null;
        var lexer = new HaibtLexer(ss);
        lexer.AddErrorListener(listener);
        var stream = new CommonTokenStream(lexer);
        var parse = new Haibt(stream);
        parse.AddErrorListener(new ErrorLister());
        Haibt.TranslationUnitContext? parseTree = parse.translationUnit();
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