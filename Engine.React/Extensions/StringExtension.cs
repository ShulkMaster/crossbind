namespace Engine.React.Extensions;

public static class StringExtension
{
    public static string Capitalize(this string ss)
    {
        if (string.IsNullOrEmpty(ss)) return string.Empty;
        if (ss.Length == 1) return ss.ToUpper();
        char cc = ss[0];
        return $"{char.ToUpper(cc)}{ss[1..]}";
    }
}