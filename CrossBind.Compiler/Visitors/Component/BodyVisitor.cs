using CrossBind.Engine.ComponentModels;
using CrossBind.Engine.StyleModel;

namespace CrossBind.Compiler.Visitors.Component;

public class BodyVisitor : HaibtBaseVisitor<ComponentBody>
{
    public override ComponentBody VisitBody(HaibtParser.BodyContext context)
    {
        var styleVisitor = new StyleVisitor();
        var body = new ComponentBody();
        foreach (var cssRule in context.css_statement())
        {
            body.BaseStyles.Add(styleVisitor.VisitCss_statement(cssRule));
        }
        
        foreach (var variant in context.variant())
        {
            Console.WriteLine(variant.GetText());
        }

        return body;
    }
}