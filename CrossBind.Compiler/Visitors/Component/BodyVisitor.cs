using CrossBind.Compiler.Symbol;
using CrossBind.Compiler.Typing;
using CrossBind.Compiler.Visitors.Properties;
using CrossBind.Engine.BaseModels;
using CrossBind.Engine.ComponentModels;

namespace CrossBind.Compiler.Visitors.Component;

public class BodyVisitor : HaibtBaseVisitor<ComponentBody>
{
    private readonly TypeManager manager = new();
    public override ComponentBody VisitBody(HaibtParser.BodyContext context)
    {
        var styleVisitor = new StyleVisitor();
        var variantVisitor = new VariantVisitor(styleVisitor);
        var propertyVisitor = new PropertyVisitor(manager, new SymbolTable(null));
        var body = new ComponentBody();
        foreach (var cssRule in context.css_statement())
        {
            body.BaseStyles.Add(styleVisitor.Visit(cssRule));
        }

        foreach (HaibtParser.PropertyContext propertyContext in context.property())
        {
            PropModel prop = propertyVisitor.Visit(propertyContext);
            body.Props.Add(prop);
        }

        foreach (HaibtParser.VariantContext? variant in context.variant())
        {
            variantVisitor.Visit(variant);
        }
        
        body.Variants.AddRange(variantVisitor.map.Values);

        return body;
    }
}