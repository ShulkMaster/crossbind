using System.Text;

namespace Engine.React.Extensions;

public static class StringBuilderExtension
{

    public static void Pop(this StringBuilder sb)
    {
        sb.Remove(sb.Length - 1, 1);
    }
}