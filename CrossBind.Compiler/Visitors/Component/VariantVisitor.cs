using Antlr4.Runtime.Tree;
using CrossBind.Engine.ComponentModels;
using CrossBind.Engine.StyleModel;
using LanguageExt;

namespace CrossBind.Compiler.Visitors.Component;

public class VariantVisitor : HaibtBaseVisitor<ComponentVariant>
{
    private readonly IHaibtVisitor<ComponentStyle> _styler;
    private HashMap<string, ComponentVariant> _map = new();

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
        _map = _map.Add(variant.Name, variant);
        return variant;
    }

    public override ComponentVariant VisitVariantInitialization(HaibtParser.VariantInitializationContext context)
    {
        ITerminalNode? name = context.IDENTIFIER();
        var variant = new ComponentVariant
        {
            Name = name?.GetText() ?? string.Empty,
        };
        _map = _map.Add(variant.Name, variant);
        foreach (HaibtParser.Variant_styleContext styleContext in context.variant_style())
        {
            VariantStyle style = VisitVariantStyle(styleContext);
            variant.Styles.Add(style);
        }

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

    public override ComponentVariant VisitVariant_ext_initialization(
        HaibtParser.Variant_ext_initializationContext context)
    {
        return base.VisitVariant_ext_initialization(context);
    }
}