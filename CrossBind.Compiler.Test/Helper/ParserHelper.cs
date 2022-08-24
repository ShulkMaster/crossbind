using Antlr4.Runtime;

namespace CrossBind.Compiler.Test.Helper;

public class ParserHelper
{
    public static HaibtParser BuildParser(string code)
    {
        var charStream = new AntlrInputStream(code);
        var lexer = new HaibtLexer(charStream);
        var stream = new CommonTokenStream(lexer);
        return new HaibtParser(stream);
    }
}