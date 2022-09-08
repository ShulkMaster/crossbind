using System.Text.RegularExpressions;
using Antlr4.Runtime;
using CrossBind.Compiler.Visitors.Component;
using CrossBind.Engine.StyleModel;

namespace CrossBind.Compiler.Test.StylesTest;

public class BasicStylesTest
{
    private const string Em = "em";
    private const string Rem = "rem";
    private const string Px = "px";

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
    [InlineData("width : 0;", Px,0f)]
    [InlineData("width: 250px;", Px, 250f)]
    [InlineData("width:30em;\n\r", Em, 30f)]
    [InlineData("width:   5rem;\n\r", Rem, 5f)]
    [InlineData(" width:5.75em;\n\r", Em, 5.75f)]
    public void Should_Parse_Width(string code, string unit, float wVal)
    {
        HaibtParser parser = BuildParser(code);
        var visitor = new StyleVisitor();
        ComponentStyle result = visitor.Visit(parser.css_statement());
        Assert.Equal($"width: {wVal}{unit};\n", result.StringValue);
    }
    
    [Theory]
    [InlineData("height : 0;", Px,0f)]
    [InlineData("height: 250px;", Px, 250f)]
    [InlineData("height:30em;\n\r", Em, 30f)]
    [InlineData("height:   5rem;\n\r", Rem, 5f)]
    [InlineData(" height:5.75em;\n\r", Em, 5.75f)]
    [InlineData(" height:5.958rem;\n\r", Rem, 5.958f)]
    public void Should_Parse_Height(string code, string unit, float wVal)
    {
        HaibtParser parser = BuildParser(code);
        var visitor = new StyleVisitor();
        ComponentStyle result = visitor.Visit(parser.css_statement());
        Assert.Equal($"height: {wVal}{unit};\n", result.StringValue);
    }
    
    [Theory]
    [InlineData("borderRadius : 0;", Px,0f)]
    [InlineData("borderRadius: 250px;", Px, 250f)]
    [InlineData("borderRadius:30em;\n\r", Em, 30f)]
    [InlineData("borderRadius:   5rem;\n\r", Rem, 5f)]
    [InlineData(" borderRadius:5.75em;\n\r", Em, 5.75f)]
    [InlineData(" borderRadius:5.958rem;\n\r", Rem, 5.958f)]
    public void Should_Parse_BorderRadius(string code, string unit, float rVal)
    {
        HaibtParser parser = BuildParser(code);
        var visitor = new StyleVisitor();
        ComponentStyle result = visitor.Visit(parser.css_statement());
        Assert.Equal($"border-radius: {rVal}{unit};\n", result.StringValue);
    }
    
    [Theory]
    [InlineData("margin : 0;", Px,0f)]
    [InlineData("margin:250  px   ;", Px, 250f)]
    [InlineData("margin:   5rem;\n\r", Rem, 5f)]
    [InlineData(" margin:5.958em;\n\r", Em, 5.958f)]
    public void Should_Parse_Single_Margin(string code, string unit, float wVal)
    {
        HaibtParser parser = BuildParser(code);
        var visitor = new StyleVisitor();
        ComponentStyle result = visitor.Visit(parser.css_statement());
        Assert.Equal($"margin: {wVal}{unit};\n", result.StringValue);
    }
    
    
    [Theory]
    [InlineData("margin : 0 2em;", Px, 0f, Em, 2f)]
    [InlineData("margin:250  px   4rem;", Px, 250f, Rem, 4f)]
    [InlineData("margin:   5rem 15.1748;\n\r", Rem, 5f, Px, 15.1748f)]
    [InlineData(" margin:5.958em 0em;\n\r", Em, 5.958f, Em, 0f)]
    public void Should_Parse_Double_Margin(string code, string unit, float mVal, string unit2, float mVal2)
    {
        HaibtParser parser = BuildParser(code);
        var visitor = new StyleVisitor();
        ComponentStyle result = visitor.Visit(parser.css_statement());
        Assert.Equal($"margin: {mVal}{unit} {mVal2}{unit2};\n", result.StringValue);
    }
    
    [Theory]
    [InlineData("margin : 2 2 2;", 2f, Px)]
    [InlineData("margin : 0em 0em 0em;", 0f, Em)]
    [InlineData("margin : 10rem 10rem 10rem;", 10f, Rem)]
    public void Should_Parse_Triple_Margin(string code, float mVal, string unit)
    {
        HaibtParser parser = BuildParser(code);
        var visitor = new StyleVisitor();
        ComponentStyle result = visitor.Visit(parser.css_statement());
        Assert.Equal($"margin: {mVal}{unit} {mVal}{unit} {mVal}{unit};\n", result.StringValue);
    }
    
    [Theory]
    [InlineData("padding : 0;", Px,0f)]
    [InlineData("padding:250  px   ;", Px, 250f)]
    [InlineData("padding:   5rem;\n\r", Rem, 5f)]
    [InlineData(" padding:5.958em;\n\r", Em, 5.958f)]
    public void Should_Parse_Single_Padding(string code, string unit, float pVal)
    {
        HaibtParser parser = BuildParser(code);
        var visitor = new StyleVisitor();
        ComponentStyle result = visitor.Visit(parser.css_statement());
        Assert.Equal($"padding: {pVal}{unit};", result.StringValue);
    }
    
    [Theory]
    [InlineData("border: 10 solid #F00", Px, 10f, "#F00", "solid")]
    [InlineData("border:250  em dashed#0FF   ;", Em, 250f, "#0FF", "dashed")]
    [InlineData("border:5rem double;\n\r", Rem, 5f, "", "double")]
    [InlineData("border: hidden #00F;\n\r", Px, 1f, "#00F", "hidden")]
    [InlineData("border: dotted;\n\r", Px, 1f, "", "dotted")]
    public void Should_Parse_Border_ShortHand(string code, string unit, float? bVal, string color, string style)
    {
        HaibtParser parser = BuildParser(code);
        var visitor = new StyleVisitor();
        Regex regex = new("\\s+");
        string expected = regex.Replace($"border: {bVal}{unit} {style} {color}".Trim(), " ");
        ComponentStyle result = visitor.Visit(parser.css_statement());
        Assert.Equal($"{expected};\n", result.StringValue);
    }
}