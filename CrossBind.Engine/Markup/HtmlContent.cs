namespace CrossBind.Engine.Markup;

public abstract class HtmlContent
{
    protected string Value { get; }

    protected HtmlContent(string value)
    {
        Value = value;
    }
}