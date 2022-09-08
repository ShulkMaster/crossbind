using System.Text;
using CrossBind.Compiler.Visitors.Style;
using CrossBind.Engine.StyleModel;

namespace CrossBind.Compiler.Visitors.Component;

public class StyleVisitor : HaibtBaseVisitor<ComponentStyle>
{
    public const string BorderRadius = "border-radius";
    private readonly ColorVisitor _color = new ();

    public override ComponentStyle VisitBgColor(HaibtParser.BgColorContext context)
    {
        string color = _color.Visit(context.color_stm());
        var visitBgColor = new ComponentStyle
        {
            Key = "background-color",
            StringValue = $"background-color: {color};",
        };
        return visitBgColor;
    }

    private BorderRule VisitBorderRule(HaibtParser.BorderValueContext context)
    {
        HaibtParser.CssMeasureContext? stroke = context.cssMeasure();

        string unit = "px";
        int width = 1;
        if (stroke is not null && stroke.ChildCount > 0)
        {
            unit = stroke.CSS_UNIT()?.GetText() ?? unit;
            string number = stroke.NUMBER().GetText();
            if (int.TryParse(number, out int newWith))
            {
                width = newWith;
            }
        }

        HaibtParser.Color_stmContext? contextColor = context.color_stm();
        string? color = null;
        if (contextColor is not null)
        {
          color  = _color.Visit(contextColor);
        }

        return new BorderRule
        {
            Color = color ?? string.Empty,
            Stroke = width,
            Unit = unit,
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

    public override ComponentStyle VisitHeight(HaibtParser.HeightContext context)
    {
        string key = context.Height().GetText();
        string measure = FromMeasure(context.cssMeasure());
        return new ComponentStyle
        {
            Key = key,
            StringValue = $"{key}: {measure};\n",
        };
    }
    
    public override ComponentStyle VisitBorderRadius(HaibtParser.BorderRadiusContext context)
    {
        string measure = FromMeasure(context.cssMeasure());
        return new ComponentStyle
        {
            Key = BorderRadius,
            StringValue = $"{BorderRadius}: {measure};\n",
        };
    }

    private static void VisitClock(HaibtParser.ClockRuleContext ctx, StringBuilder sb)
    {
        foreach (HaibtParser.CssMeasureContext? rule in ctx.cssMeasure())
        {
            sb.Append(FromMeasure(rule));
            sb.Append(' ');
        }

        sb.Remove(sb.Length - 1, 1);
    }

    public override ComponentStyle VisitMargin(HaibtParser.MarginContext context)
    {
        var sb = new StringBuilder("margin: ");
        VisitClock(context.clockRule(), sb);
        sb.Append(";\n");
        return new ComponentStyle
        {
            Key = "margin",
            StringValue = sb.ToString(),
        };
    }
    
    public override ComponentStyle VisitPadding(HaibtParser.PaddingContext context)
    {
        var sb = new StringBuilder("padding: ");
        VisitClock(context.clockRule(), sb);
        sb.Append(';');
        return new ComponentStyle
        {
            Key = "padding",
            StringValue = sb.ToString(),
        };
    }
}