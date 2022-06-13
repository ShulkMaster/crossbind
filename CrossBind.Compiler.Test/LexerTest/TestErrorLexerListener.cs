using Antlr4.Runtime;
using Xunit.Abstractions;

namespace CrossBind.Compiler.Test.LexerTest;

public class TestErrorLexerListener: IAntlrErrorListener<int>
{
    public TestErrorLexerListener(ITestOutputHelper console)
    {
        Out = console;
    }

    public void SyntaxError(TextWriter output, IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine,
        string msg, RecognitionException e)
    {
        Out.WriteLine($"Error [{line}, {charPositionInLine}]: {offendingSymbol}");
        Out.WriteLine(msg);
    }

    public ITestOutputHelper Out { get; }
}