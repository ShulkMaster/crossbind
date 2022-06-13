using Antlr4.Runtime;

namespace CrossBind.Compiler.Test;

public static class Extension
{
    public static IEnumerable<IToken> FilterWhiteSpace(this IEnumerable<IToken> tokens)
    {
        return tokens.Filter(token => token.Text is not ("\n" or "\r" or " " or "\t"));
    }
}