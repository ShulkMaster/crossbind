namespace CrossBind.Compiler.Test.Samples;

public static class HexSample
{
    public const string InvalidRedHexCode = "#f00";
    public const string ValidRedHexCode = "#F00";
    public const string ValidRedLongHexCode = "#FF0000";
    public const string InvalidGreenHexCode = "#0f0";
    public const string ValidGreenHexCode = "#0F0";
    public const string ValidGreenLongHexCode = "#00FF00";
    public const string ValidBlueLongHexCode = "#0000FF";
    public const string InvalidHexCode1 = "#4aB";
    public const string ValidLongHexCode1 = "#44AABB";
    public const string InvalidHexCode2 = "#43544c";
    public const string InvalidHexCode3 = "#43544";
}