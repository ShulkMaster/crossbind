using CrossBind.Engine.StyleModel;

namespace CrossBind.Compiler.Visitors.Component;

public class StyleVisitor : HaibtBaseVisitor<ComponentStyle>
{
    public override ComponentStyle VisitBgColor(HaibtParser.BgColorContext context)
    {
        var visitBgColor = new ComponentStyle
        {
            Key = "background-color",
            StringValue = context.HEX_COLOR().GetText(),
        };
        return visitBgColor;
    }

    public override ComponentStyle VisitInlineBorder(HaibtParser.InlineBorderContext context)
    {
        var border = new ComponentStyle
        {
            Key = StyleBorder.BorderKey,
            StringValue = $"{context.NUMBER().GetText()}px {context.IDENTIFIER().GetText()} {context.HEX_COLOR()}",
        };
        return border;
    }

    public override ComponentStyle VisitCompoundBorder(HaibtParser.CompoundBorderContext context)
    {
        int count = context.ChildCount;
        var nodes = new string[count - 4];
        if (nodes.Length > 0)
        {
            for (var i = 0; i < nodes.Length; i++)
            {
                nodes[i] = context.GetChild(i + 3).GetText();
            }
        }
        string? style = nodes.Reduce((prev, current) => prev+ " " + current);

        var border = new StyleBorder
        {
            Key = StyleBorder.BorderKey,
            StringValue = style,
        };
        return border;
    }
}