using CrossBind.Engine.ComponentModels;

namespace CrossBind.Compiler.Visitors.Component;

public class BodyVisitor : HaibtBaseVisitor<ComponentBody>
{
    public override ComponentBody VisitBody(HaibtParser.BodyContext context)
    {
        var styleVisitor = new StyleVisitor();
        var body = new ComponentBody();
        foreach (var cssRule in context.css_statement())
        {
            body.BaseStyles.Add(styleVisitor.Visit(cssRule));
        }

        return body;
    }
}