using System.Text;

namespace CrossBind.Engine.StyleModel;

public enum StyleBorderType
{
    Solid,
    Dashed,
    Dotted
}

public record BorderRule
{
    private const string Solid = "solid";

    public int Stroke { get; init; } = 1;
    public string BorderType { get; init; } = Solid;
    public string Color { get; init; } = string.Empty;
    public string Unit { get; init; } = string.Empty;

    public string AsBorder()
    {
        return $"{Stroke}{Unit} {BorderType} {Color}".Trim();
    }
}

public class StyleBorder : ComponentStyle
{
    private readonly string _stringValue = string.Empty;
    private readonly string[] _borderKeys = { "border-top", "border-right", "border-left", "border-bottom" };

    #region Constants

    private const int Top = 0;
    private const int Right = 1;
    private const int Bottom = 2;
    private const int Left = 3;

    #endregion

    public const string BorderKey = "border";
    public BorderRule?[] Borders { get; }

    public StyleBorder(BorderRule[] borders)
    {
        Borders = borders;
    }

    public StyleBorder(BorderRule border)
    {
        Borders = new[] { border, border,border,border };
    }



    public override string StringValue
    {
        get
        {
            bool isHorizontal = Borders[Left] == Borders[Right];
            bool isVertical = Borders[Left] == Borders[Right];
            BorderRule? anyBorder = Borders[0];
            if (isHorizontal && isVertical)
            {
                return $"{BorderKey} : {anyBorder?.AsBorder()};";
            }

            var sb = new StringBuilder();
            for (int i = 0; i < Borders.Length; i++)
            {
                if (Borders[i] is null) continue;
                sb.Append(_borderKeys[i]);
                sb.Append(" : ");
                sb.Append(Borders[i]!.AsBorder());
                sb.Append(";\n");
            }

            return sb.ToString();
        }
        init => _stringValue = value;
    }
}