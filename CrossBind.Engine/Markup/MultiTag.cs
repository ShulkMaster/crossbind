namespace CrossBind.Engine.Markup;

public sealed class MultiTag: Tag
{
    public MultiTag(IEnumerable<HtmlContent> contents): base("Fragment")
    {
        Content = contents;
    }

}