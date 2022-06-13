using Antlr4.Runtime;

namespace CrossBind.Compiler.Error;

public class HaibtErrorStrategy: DefaultErrorStrategy
{
    public override void ReportError(Parser recognizer, RecognitionException e)
    {
        Console.WriteLine(recognizer.CurrentToken.Line);
        Console.WriteLine(e);
        base.ReportError(recognizer, e);
    }
}