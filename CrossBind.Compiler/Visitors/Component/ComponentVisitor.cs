using CrossBind.Engine.ComponentModels;

namespace CrossBind.Compiler.Visitors.Component;

public class ComponentVisitor: HaibtBaseVisitor<ComponentModel>
{

    public static Extendable ToExtendable(string? extend)
    {
        return extend?.ToUpper() switch
        {
            nameof(Extendable.Button) => Extendable.Button,
            nameof(Extendable.Select) => Extendable.Select,
            nameof(Extendable.TextBox) => Extendable.TextBox,
            _ => Extendable.Component,
        };
    }


    public override ComponentModel VisitCompDeclaration(HaibtParser.CompDeclarationContext context)
    {
        string? extends = context.CANNON_COMP().GetText();
        var model = new ComponentModel(ToExtendable(extends))
        {
            Name = context.IDENTIFIER().GetText(),
            Body = context.body().GetText()
        };
        return model;
    }
}