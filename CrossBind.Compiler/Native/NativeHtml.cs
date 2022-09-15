namespace CrossBind.Compiler.Native;

public static class NativeHtml
{
    private static readonly HashSet<string> NativeTags = new();

    static NativeHtml()
    {
        foreach (string tag in TagList.LayoutTags)
        {
            NativeTags.Add(tag);
        }

        foreach (string tag in TagList.ContentTags)
        {
            NativeTags.Add(tag);
        }
    }

    public static bool IsNative(string tagName)
    {
        return NativeTags.Contains(tagName);
    }
}