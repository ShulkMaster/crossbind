using Antlr4.Runtime;
using CrossBind.Compiler.Error;
using static CrossBind.Compiler.Test.Samples.ButtonSample;
using static CrossBind.Compiler.Test.Samples.HexSample;
using Xunit.Abstractions;

namespace CrossBind.Compiler.Test.LexerTest;

public class LexerErrorTest
{
    private readonly TestErrorLexerListener _listener;
    private const int NonSenseTokensNoWs = 12;

    private const string NonSense = @"
this is non sense works
just to
see

How
the Lexers
Behaves";

    public LexerErrorTest(ITestOutputHelper console)
    {
        _listener = new TestErrorLexerListener(console);
    }

    [Fact]
    public void Should_Report_Zero_Tokens()
    {
        var code = string.Empty;
        var charStream = new AntlrInputStream(code);
        var lexer = new HaibtLexer(charStream);
        lexer.AddErrorListener(_listener);
        var tokens = lexer.GetAllTokens();
        Assert.Empty(tokens);
    }

    [Fact]
    public void Should_Report_Anything_As_Token()
    {
        var charStream = new AntlrInputStream(NonSense);
        var lexer = new HaibtLexer(charStream);
        lexer.AddErrorListener(_listener);
        var tokens = lexer.GetAllTokens().FilterWhiteSpace();
        Assert.Equal(NonSenseTokensNoWs, tokens.Count());
    }

    [Theory]
    [InlineData(SimpleValidButton, SimpleValidButtonTokensNoWs)]
    [InlineData(NonSense, NonSenseTokensNoWs)]
    public void Should_Report_N_Tokens(string code, int count)
    {
        var charStream = new AntlrInputStream(code);
        var lexer = new HaibtLexer(charStream);
        lexer.AddErrorListener(_listener);
        var tokens = lexer.GetAllTokens().FilterWhiteSpace();
        Assert.Equal(count, tokens.Count());
    }

    [Fact]
    public void Should_Report_NonSense()
    {
        /*
         * The NonSense string will match all the words as the identifier rule, thus being valid syntax
         * input with 12 valid identifiers
         */
        var charStream = new AntlrInputStream(NonSense);
        var lexer = new HaibtLexer(charStream);
        var tokens = lexer.GetAllTokens().FilterWhiteSpace();
        Assert.Equal(NonSenseTokensNoWs, tokens.Count());
    }

    [Theory]
    [InlineData(InvalidRedHexCode)]
    [InlineData(InvalidGreenHexCode)]
    [InlineData(InvalidHexCode1)]
    [InlineData(InvalidHexCode2)]
    [InlineData(InvalidHexCode3)]
    public void Should_Report_Error_With_LowerCase_HexCodes(string hexCode)
    {
        var listener = new HaibtLexerErrorListener();
        var charStream = new AntlrInputStream(hexCode);
        var lexer = new HaibtLexer(charStream);
        lexer.AddErrorListener(listener);
        var tokens = lexer.GetAllTokens();
        var errors = listener.GetErrors();
        // Lexer will try to match as much as possible so invalid code as #43544
        // are interpreted as valid Hex #435 follow by '44' token thus creating more
        // than a single token
        if (tokens.Count == 1)
        {
            Assert.Single(errors);
        }
    }
    
    [Theory]
    [InlineData(ValidRedLongHexCode)]
    [InlineData(ValidGreenLongHexCode)]
    [InlineData(ValidBlueLongHexCode)]
    [InlineData(ValidLongHexCode1)]
    public void Should_Report_Valid_Long_HexCodes(string hexCode)
    {
        var listener = new HaibtLexerErrorListener();
        var charStream = new AntlrInputStream(hexCode);
        var lexer = new HaibtLexer(charStream);
        lexer.AddErrorListener(listener);
        var tokens = lexer.GetAllTokens();
        var errors = listener.GetErrors();
        foreach (var token in tokens)
        {
            _listener.Out.WriteLine(token.Text);
        }
        Assert.Single(tokens);
        Assert.Empty(errors);
    }
}