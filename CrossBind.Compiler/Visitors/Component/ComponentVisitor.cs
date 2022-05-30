using CrossBind.Engine.ComponentModels;

namespace CrossBind.Compiler.Visitors.Component;

public class ComponentVisitor: HaibtBaseVisitor<ComponentModel>
{
    private static Extendable ToExtendable(string? extend)
    {
        return extend switch
        {
            "button" => Extendable.Button,
            "select" => Extendable.Select,
            "textbox" => Extendable.TextBox,
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