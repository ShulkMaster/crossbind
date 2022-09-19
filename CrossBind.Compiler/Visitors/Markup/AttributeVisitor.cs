using Antlr4.Runtime.Tree;
using CrossBind.Compiler.Parser;
using CrossBind.Engine.Markup;
using CrossBind.Engine.Types;

namespace CrossBind.Compiler.Visitors.Markup;

public class AttributeVisitor: HaibtBaseVisitor<AttributeModel>
{
    public override AttributeModel VisitAttributeAssign(Haibt.AttributeAssignContext context)
    {
        string name = context.IDENTIFIER().GetText();
        Haibt.HtmlAttributeValueContext attribVal = context.htmlAttributeValue();
        ITerminalNode? node = attribVal.StringLiteral();
        if (node is not null)
        {
            return new ConstAttributeModel(name, Primitive.String(false))
            {
                ConsValue = node.GetText()
            };
        }

        node = attribVal.AttributeValue();
        if (node is not null)
        {
            return new ConstAttributeModel(name, Primitive.String(false))
            {
                ConsValue = node.GetText()
            };
        }

        string text = attribVal.objectExpressionSequence().GetText();
        return new AssignAttributeModel(name, new StringLiteralType("", ""))
        {
            Bind = true,
            Identifier = text,
        };
    }

    public override AttributeModel VisitAttributeBoolean(Haibt.AttributeBooleanContext context)
    {
        string name = context.IDENTIFIER().GetText();
        return new ConstAttributeModel(name, Primitive.Bool(false))
        {
            ConsValue = "true"
        };
    }

    public override AttributeModel VisitAttributeBind(Haibt.AttributeBindContext context)
    {
        string name = context.IDENTIFIER().GetText();
        return new AssignAttributeModel(name, new ObjectType("something", "", false))
        {
            Bind = true,
            Identifier = name,
        };
    }
}