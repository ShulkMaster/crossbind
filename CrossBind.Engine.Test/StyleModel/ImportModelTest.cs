using CrossBind.Engine.StyleModel;


namespace CrossBind.Engine.Test.StyleModel;

public class StyleModelTest
{
    [Fact]
    public void Should_Return_As_CSS()
    {
        var style = new ComponentStyle
        {
            Key = "width",
            StringValue = "width: 16px;",
        };

        Assert.Equal("width: 16px;", style.StringValue);
    }
}