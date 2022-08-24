using Antlr4.Runtime.Tree;
using CrossBind.Engine.ComponentModels;
using CrossBind.Engine.StyleModel;

namespace CrossBind.Compiler.Visitors.Component;

public class VariantVisitor : HaibtBaseVisitor<ComponentVariant>
{
    private readonly IHaibtVisitor<ComponentStyle> _styler;
    public readonly Dictionary<string, ComponentVariant> map = new();

    public VariantVisitor(IHaibtVisitor<ComponentStyle> styler)
    {
        _styler = styler;
    }

    public override ComponentVariant VisitVariantDeclaration(HaibtParser.VariantDeclarationContext context)
    {
        ITerminalNode? name = context.IDENTIFIER();
        var variant = new ComponentVariant
        {
            Name = name?.GetText() ?? string.Empty,
        };
        map.Add(variant.Name, variant);
        return variant;
    }

    public override ComponentVariant VisitVariantInitialization(HaibtParser.VariantInitializationContext context)
    {
        string name = context.IDENTIFIER()?.GetText() ?? string.Empty;
        ComponentVariant variant = map[name];
        VariantStyle style = VisitVariantStyle(context.variant_style());
        variant.Styles.Add(style);

        return variant;
    }

    private VariantStyle VisitVariantStyle(HaibtParser.Variant_styleContext context)
    {
        string name = context.IDENTIFIER().GetText() ?? string.Empty;
        var variant = new VariantStyle
        {
            ValueKey = name,
        };

        foreach (HaibtParser.Css_statementContext statements in context.css_statement())
        {
            ComponentStyle style = _styler.Visit(statements);
            variant.VariantStyles.Add(style);
        }
        return variant;
    }
    
}