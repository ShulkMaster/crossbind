using CrossBind.Engine.StyleModel;

namespace CrossBind.Compiler.Visitors.Component;

public class StyleVisitor: HaibtBaseVisitor<ComponentStyle>
{
    public override ComponentStyle VisitCss_statement(HaibtParser.Css_statementContext context)
    {
        if (context.BackgroundColor().ChildCount > 0)
        {
            return new ComponentStyle
            {
                StringValue = context.BackgroundColor().GetChild(2).GetText(),
                Key = context.BackgroundColor().GetChild(0).GetText(),
            };
        }

        if (context.Border().ChildCount > 0)
        {
            return new StyleBorder();
        }

        return new ComponentStyle();
    }
}