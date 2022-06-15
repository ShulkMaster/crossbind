namespace CrossBind.Engine.StyleModel;

public enum StyleBorderType
{
    Solid,
    Dashed,
    Dotted
}

public record BorderRule
{
    public int Stroke { get; init; } = 1;
    public StyleBorderType BorderType { get; init; } = StyleBorderType.Solid;
    public string Color { get; init; } = string.Empty;
}

public class StyleBorder : ComponentStyle
{
    private readonly string _stringValue = string.Empty;

    #region Constants

    private const int Top = 0;
    private const int Right = 1;
    private const int Bottom = 2;
    private const int Left = 3;
    public const string BorderKey = "border";

    #endregion

    public BorderRule?[] GetBorder { get; } = { null, null, null, null };

    public override string StringValue
    {
        get
        {
            bool isHorizontal = GetBorder[Left] == GetBorder[Right];
            bool isVertical = GetBorder[Left] == GetBorder[Right];
            var anyBorder = GetBorder[0];
            if (isHorizontal && isVertical)
            {
                return $"{BorderKey} : {anyBorder?.Stroke}px {anyBorder?.Color} {anyBorder?.BorderType.ToString().ToLower()}";
            }
            
            return $"{BorderKey} : undefined;";
        }
        init => _stringValue = value;
    }

    public void SetTop(BorderRule topBorder)
    {
        GetBorder[Top] = topBorder;
    }
    
    public void SetBottom(BorderRule bottomBorder)
    {
        GetBorder[Bottom] = bottomBorder;
    }
    
    public void SetLeft(BorderRule leftBorder)
    {
        GetBorder[Left] = leftBorder;
    }
    
    public void SetRight(BorderRule rightBorder)
    {
        GetBorder[Right] = rightBorder;
    }
    
    public void SetVertical(BorderRule border)
    {
        SetTop(border);
        SetBottom(border);
    }
    
    public void SetHorizontal(BorderRule border)
    {
        SetLeft(border);
        SetRight(border);
    }
    
    public void SetBorder(BorderRule border)
    {
        SetVertical(border);
        SetHorizontal(border);
    }

}