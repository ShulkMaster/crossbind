using System.Text;
using Antlr4.Runtime.Tree;
using CrossBind.Compiler.Native;
using CrossBind.Compiler.Parser;
using CrossBind.Engine.ComponentModels;
using CrossBind.Engine.Markup;

namespace CrossBind.Compiler.Visitors.Markup;

public class TagVisitor : HaibtBaseVisitor<Tag>
{
    public override Tag VisitMarkup(Haibt.MarkupContext? context)
    {
        if(context is null) return NoTag.Instance;
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


        return new ComponentTag(new ComponentModel())
        {
            Content = htmlContents,
            Attributes = attribs
        };
    }

    public override Tag VisitHtmlOptional(Haibt.HtmlOptionalContext context)
    {
        ITerminalNode? tags = context.IDENTIFIER();
        string tName = tags?.GetText() ?? string.Empty;
        bool isNative = NativeHtml.IsNative(tName);
        var attribs = VisitAttribs(context.htmlAttribute());

        if (isNative)
        {
            NativeTag native = new(tName)
            {
                Attributes = attribs
            };
            return native;
        }

        return new ComponentTag(new ComponentModel
        {
            Name = tName
        })
        {
            Attributes = attribs,
        };
    }

    public override Tag VisitHtmlSingle(Haibt.HtmlSingleContext context)
    {
        ITerminalNode? tags = context.IDENTIFIER();
        string tName = tags?.GetText() ?? string.Empty;
        bool isSingleTag = tName == "img";
        var attribs = VisitAttribs(context.htmlAttribute());

        if (isSingleTag)
        {
            NativeTag native = new(tName)
            {
                Attributes = attribs
            };
            return native;
        }

        return NoTag.Instance;
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

            int children = child.ChildCount;
            StringBuilder acumm = new();
            for (int i = 0; i < children; i++)
            {
                IParseTree xd = child.GetChild(i);
                acumm.Append(xd.GetText());
                acumm.Append(' ');
            }

            contents.Add(new HtmlText(acumm.ToString()));
        }

        return contents;
    }

    private static List<AttributeModel> VisitAttribs(Haibt.HtmlAttributeContext[] attribs)
    {
        var list = new List<AttributeModel>(attribs.Length);
        var attribVisitor = new AttributeVisitor();
        list.AddRange(attribs.Select(
            attrib => attribVisitor.Visit(attrib)
        ));
        return list;
    }
}