using CrossBind.Engine.BaseModels;

namespace CrossBind.Engine.Markup;

public abstract class Tag: HtmlContent
{
    public string Name { get; }
    public IEnumerable<HtmlContent> Content { get; init; } = Array.Empty<HtmlContent>();
    public List<PropModel> Attributes { get; init; } = new();

    protected Tag(string name): base(string.Empty)
    {
        Name = name;
    }

}