using Antlr4.Runtime.Tree;
using CrossBind.Compiler.Native;
using CrossBind.Compiler.Parser;
using CrossBind.Engine.BaseModels;
using CrossBind.Engine.ComponentModels;
using CrossBind.Engine.Markup;
using CrossBind.Engine.Types;

namespace CrossBind.Compiler.Visitors.Markup;

public class TagVisitor : HaibtBaseVisitor<Tag>
{
    public override Tag VisitMarkup(Haibt.MarkupContext context)
    {
        var elements = context.htmlElement();
        return elements.Length switch
        {
            0 => NoTag.Instance,
            1 => Visit(elements[0]),
            _ => VisitMultiTag(elements)
        };
    }

    private Tag VisitMultiTag(IEnumerable<Haibt.HtmlElementContext> context)
    {
        IEnumerable<HtmlContent> tags = context.Select(Visit);
        return new MultiTag(tags);
    }

    public override Tag VisitHtmlChildren(Haibt.HtmlChildrenContext context)
    {
        var tags = context.IDENTIFIER();
        foreach (ITerminalNode terminalNode in tags)
        {
            Console.WriteLine(terminalNode.GetText());
        }

        string tName = tags.First().GetText();
        Haibt.HtmlContentContext content = context.htmlContent();
        var attribs = VisitAttribs(context.htmlAttribute());
        var htmlContents = VisitHtmlContents(content);
        bool isNative = NativeHtml.IsNative(tName);

        if (isNative)
        {
            NativeTag native = new(tName)
            {
                Content = htmlContents,
                Attributes = attribs
            };
            return native;
        }


        return new ComponentTag(new ComponentModel());
    }

    public override Tag VisitHtmlOptional(Haibt.HtmlOptionalContext context)
    {
        return base.VisitHtmlOptional(context);
    }

    public override Tag VisitHtmlSingle(Haibt.HtmlSingleContext context)
    {
        return base.VisitHtmlSingle(context);
    }

    private IEnumerable<HtmlContent> VisitHtmlContents(Haibt.HtmlContentContext context)
    {
        var contents = new List<HtmlContent>(context.children.Count);
        foreach (IParseTree? child in context.children)
        {
            Tag? t = Visit(child);
            if (t is not null)
            {
                contents.Add(t);
                continue;
            }
            contents.Add(new HtmlText(child.GetText()));
        }
        return contents;
    }

    private static List<PropModel> VisitAttribs(Haibt.HtmlAttributeContext[] attribs)
    {
        var x = attribs.Select(a => new PropModel(
            a.IDENTIFIER().GetText()
            , Primitive.String(false)
        ));
        return x.ToList();
    }
}