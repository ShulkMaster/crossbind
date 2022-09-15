using Antlr4.Runtime.Tree;
using CrossBind.Compiler.Parser;
using CrossBind.Compiler.Test.Helper;
using CrossBind.Compiler.Test.Samples;
using CrossBind.Compiler.Visitors.Component;
using CrossBind.Engine.StyleModel;
using Moq;

namespace CrossBind.Compiler.Test.VariantTest;

public class StyleVariant
{
    
    [Fact]
    public void Should_Register_Variant()
    {
        const string code = VariantSample.VariantDeclaration;
        Haibt parser = ParserHelper.BuildParser(code);
        var mock = new Mock<IHaibtVisitor<ComponentStyle>>();
        var visitor = new VariantVisitor(mock.Object);
        visitor.Visit(parser.variant());
        var m = visitor.map;
        Assert.Single(m);
        Assert.Equal(VariantSample.VarName, m.First().Key);
    }
    
    [Fact]
    public void Should_Create_Variant_Style()
    {
        const string declaration = VariantSample.VariantDeclaration;
        const string variant = VariantSample.VariantInitialization;
        Haibt parser = ParserHelper.BuildParser(declaration +  variant);
        var mock = new Mock<IHaibtVisitor<ComponentStyle>>();
        var visitor = new VariantVisitor(mock.Object);
        visitor.Visit(parser.body());
        mock.Verify(m => m.Visit(It.IsAny<IParseTree>()), Times.Exactly(2));
    }
}