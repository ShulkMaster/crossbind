using Antlr4.Runtime.Tree;
using CrossBind.Compiler.Parser;
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

    public override ComponentVariant VisitVariantDeclaration(Haibt.VariantDeclarationContext context)
    {
        ITerminalNode[] identifiers = context.IDENTIFIER();
        ITerminalNode name = identifiers[0];
        ITerminalNode? defaultNode = null;
        if (identifiers.Length > 1)
        {
            defaultNode = identifiers[1];
        }

        var variant = new ComponentVariant
        {
            Name = name.GetText() ?? string.Empty,
            DefaultName = defaultNode?.GetText()
        };
        map.Add(variant.Name, variant);
        return variant;
    }

    public override ComponentVariant VisitVariantInitialization(Haibt.VariantInitializationContext context)
    {
        string name = context.IDENTIFIER()?.GetText() ?? string.Empty;
        ComponentVariant variant = map[name];
        VariantStyle style = VisitVariantStyle(context.variant_style());
        variant.Styles.Add(style);
        style.Default = style.ValueKey == variant.DefaultName;
        return variant;
    }

    private VariantStyle VisitVariantStyle(Haibt.Variant_styleContext context)
    {
        string name = context.IDENTIFIER().GetText() ?? string.Empty;
        var variant = new VariantStyle
        {
            ValueKey = name,
        };

        foreach (Haibt.Css_statementContext statements in context.css_statement())
        {
            ComponentStyle style = _styler.Visit(statements);
            variant.VariantStyles.Add(style);
        }

        return variant;
    }
}