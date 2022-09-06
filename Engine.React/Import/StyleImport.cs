using System.Text;

namespace Engine.React.Import;

public sealed class StyleImport : ReactImport
{
    public StyleImport(string path) : base(path)
    {
    }

    public override void WriteStatement(StringBuilder sb, int indent)
    {
        sb.Append(' ', indent);
        sb.Append($"import '{ModulePath}';");
    }
}