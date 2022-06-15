using Antlr4.Runtime;

namespace CrossBind.Compiler.Error;

public class ErrorLister: BaseErrorListener
{
    public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine,
        string msg, RecognitionException e)
    {
        Console.Out.WriteLine(msg);
    }
}