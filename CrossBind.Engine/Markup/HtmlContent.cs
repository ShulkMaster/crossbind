namespace CrossBind.Engine.Markup;

public abstract class HtmlContent
{
    private string Value { get; }

    protected HtmlContent(string value)
    {
        Value = value;
    }
}