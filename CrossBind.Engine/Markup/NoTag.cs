namespace CrossBind.Engine.Markup;

public sealed class NoTag : Tag
{
    public static readonly NoTag Instance = new();

    private NoTag() : base(string.Empty)
    {
    }
}