using Antlr4.Runtime;
using CrossBind.Parser.Implementation;

namespace CrossBind.Compiler.Test.Helper;

public static class ParserHelper
{
    public static Haibt BuildParser(string code)
    {
        var charStream = new AntlrInputStream(code);
        var lexer = new HaibtLexer(charStream);
        var stream = new CommonTokenStream(lexer);
        return new Haibt(stream);
    }
}