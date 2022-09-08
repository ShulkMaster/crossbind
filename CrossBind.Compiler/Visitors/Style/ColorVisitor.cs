namespace CrossBind.Compiler.Visitors.Style;

public class ColorVisitor: HaibtBaseVisitor<string>
{
    public override string VisitConsColor(HaibtParser.ConsColorContext context)
    {
        string baseColor = context.HEX_COLOR().GetText().ToUpper();
        if (context.ChildCount == 3)
        {
            Console.WriteLine($"{context.SHADES().GetText()}");  
        }
        if (context.ChildCount == 4)
        {
            Console.WriteLine($"{context.SING().GetText()} {context.SHADES().GetText()}");
        }
        return baseColor;
    }
}