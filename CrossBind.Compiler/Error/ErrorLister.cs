using Antlr4.Runtime;

namespace CrossBind.Compiler.Error;

public class ErrorLister: BaseErrorListener
{
    public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine,
        string msg, RecognitionException e)
    {
        Console.WriteLine(output.Encoding);
        Console.Out.WriteLine(recognizer.GrammarFileName);
        Console.Out.WriteLine(offendingSymbol.Text, offendingSymbol.Column, offendingSymbol.Line);
        Console.Out.WriteLine(line);
        Console.Out.WriteLine(charPositionInLine);
        Console.Out.WriteLine(msg);
        Console.Out.WriteLine(e);
        base.SyntaxError(output, recognizer, offendingSymbol, line, charPositionInLine, msg, e);
    }
}