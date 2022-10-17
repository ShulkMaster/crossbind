using CrossBind.Parser.Implementation;
using CrossBind.Compiler.Symbol;
using CrossBind.Compiler.Typing;
using CrossBind.Compiler.Visitors.Markup;
using CrossBind.Compiler.Visitors.Properties;
using CrossBind.Engine.BaseModels;
using CrossBind.Engine.ComponentModels;
using CrossBind.Engine.Markup;
using CrossBind.Engine.StyleModel;

namespace CrossBind.Compiler.Visitors.Component;

public class BodyVisitor : HaibtBaseVisitor<ComponentBody>
{
    private readonly TypeManager manager = new();
    public override ComponentBody VisitBody(Haibt.BodyContext context)
    {
        var styleVisitor = new StyleVisitor();
        var variantVisitor = new VariantVisitor(styleVisitor);
        var propertyVisitor = new PropertyVisitor(manager, new SymbolTable(null));
        List<ComponentStyle> baseStyles = new();
        foreach (var cssRule in context.css_statement())
        {
            baseStyles.Add(styleVisitor.Visit(cssRule));
        }

        List<PropModel> props = new();
        foreach (Haibt.PropertyContext propertyContext in context.property())
        {
            PropModel prop = propertyVisitor.Visit(propertyContext);
            props.Add(prop);
        }

        foreach (Haibt.VariantContext? variant in context.variant())
        {
            variantVisitor.Visit(variant);
        }
        
        TagVisitor tv = new();
        Tag markup = tv.VisitMarkup(context.markup());

        return new ComponentBody
        {
            BaseStyles = baseStyles,
            Props = props,
            Variants = variantVisitor.map.Values.ToList(),
            Html = markup,
        };
    }
}