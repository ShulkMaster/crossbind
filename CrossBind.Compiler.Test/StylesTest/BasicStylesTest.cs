using Antlr4.Runtime;
using CrossBind.Compiler.Visitors.Component;
using CrossBind.Engine.StyleModel;

namespace CrossBind.Compiler.Test.StylesTest;

public class BasicStylesTest
{

    private static HaibtParser BuildParser(string code)
    {
        var charStream = new AntlrInputStream(code);
        var lexer = new HaibtLexer(charStream);
        var stream = new CommonTokenStream(lexer);
        return new HaibtParser(stream);
    }
    
    [Theory]
    [InlineData("zIndex : 5;")]
    [InlineData("zIndex:5;")]
    [InlineData("zIndex:5;\n\r")]
    [InlineData("zIndex:   5;\n\r")]
    [InlineData(" zIndex:5;\n\r")]
    public void Should_Parse_Z_Index(string code)
    {
        HaibtParser parser = BuildParser(code);
        var visitor = new StyleVisitor();
        ComponentStyle result = visitor.Visit(parser.css_statement());
        Assert.Equal("z-index: 5;\n", result.StringValue);
    }
    
    [Theory]
    [InlineData("width : 0;", "px",0f)]
    [InlineData("width: 250px;", "px", 250f)]
    [InlineData("width:30em;\n\r", "em", 30f)]
    [InlineData("width:   5rem;\n\r", "rem", 5f)]
    [InlineData(" width:5.75em;\n\r", "em", 5.75f)]
    public void Should_Parse_Width(string code, string unit, float wVal)
    {
        HaibtParser parser = BuildParser(code);
        var visitor = new StyleVisitor();
        ComponentStyle result = visitor.Visit(parser.css_statement());
        Assert.Equal($"width: {wVal}{unit};\n", result.StringValue);
    }
}