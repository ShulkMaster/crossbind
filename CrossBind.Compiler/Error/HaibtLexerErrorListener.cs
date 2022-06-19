using Antlr4.Runtime;
namespace CrossBind.Compiler.Error;

public class HaibtLexerErrorListener: IAntlrErrorListener<int>
{
    private readonly List<string> _errorList = new();


    public void SyntaxError(TextWriter output, IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine,
        string msg, RecognitionException e)
    {
        _errorList.Add(msg);
    }

    public IEnumerable<string> GetErrors() => _errorList;
}