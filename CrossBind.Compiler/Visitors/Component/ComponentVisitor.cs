using CrossBind.Parser.Implementation;
using CrossBind.Engine.ComponentModels;

namespace CrossBind.Compiler.Visitors.Component;

public class ComponentVisitor: HaibtBaseVisitor<ComponentModel>
{

    public override ComponentModel VisitCompDeclaration(Haibt.CompDeclarationContext context)
    {
        ComponentBody body = new BodyVisitor().VisitBody(context.body());
        var model = new ComponentModel
        {
            Name = context.IDENTIFIER().GetText(),
            Body = body,
        };
        return model;
    }
}