namespace CrossBind.Engine.Markup;

public sealed class HtmlText: HtmlContent
{
    public HtmlText(string text): base(text)
    {
    }

    public string GetCuratedString()
    {
        return string.Empty;
    }
}