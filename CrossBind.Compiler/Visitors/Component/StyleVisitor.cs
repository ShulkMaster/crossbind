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

    private static BorderRule VisitBorderRule(HaibtParser.BorderValueContext context)
    {
        var stroke = context.cssMeasure();
        string? strokeWidth = stroke.NUMBER()?.GetText();
        int width = 1;
        if (!string.IsNullOrWhiteSpace(strokeWidth))
        {
            width = int.Parse(strokeWidth);
        }
        
        return new BorderRule
        {
            Color = context.HEX_COLOR()?.GetText() ?? string.Empty,
            Stroke = width,
            Unit = stroke.CSS_UNIT()?.GetText() ?? "px",
            BorderType = context.BORDER_STYLE().GetText(),
        };
    }

    public override ComponentStyle VisitInlineBorder(HaibtParser.InlineBorderContext context)
    {
        BorderRule value = VisitBorderRule(context.borderValue());
        var border = new StyleBorder(value);
        return border;
    }

    public override ComponentStyle VisitCompoundBorder(HaibtParser.CompoundBorderContext context)
    {
        var borderValues = context.borderValue();
        var borders = new BorderRule[4];
        for (int index = 0; index < borderValues.Length; index++)
        {
            var borderValue = borderValues[index];
            borders[index] = VisitBorderRule(borderValue);
        }

        var border = new StyleBorder(borders);
        return border;
    }

    public override ComponentStyle VisitZIndex(HaibtParser.ZIndexContext context)
    {
        return new ComponentStyle
        {
            Key = "z-index",
            StringValue = $"z-index: {context.NUMBER().GetText()};\n",
        };
    }

    private static string FromMeasure(HaibtParser.CssMeasureContext ctx)
    {
        string unit = ctx.CSS_UNIT()?.GetText() ?? "px";
        return ctx.NUMBER().GetText() + unit;
    }

    public override ComponentStyle VisitWidth(HaibtParser.WidthContext context)
    {
        string key = context.Width().GetText();
        string measure = FromMeasure(context.cssMeasure());
        return new ComponentStyle
        {
            Key = key,
            StringValue = $"{key}: {measure};\n",
        };
    }
}