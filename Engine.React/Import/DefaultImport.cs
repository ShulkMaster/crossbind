using System.Text;

namespace Engine.React.Import;

public sealed class DefaultImport : ReactImport
{
    public string ModuleName { get; init; } = string.Empty;
    public SortedSet<string> Symbols { get; } = new();

    public DefaultImport(string path) : base(path)
    {
    }

    public override void WriteStatement(StringBuilder sb, int indent)
    {
        sb.Append(' ', indent);
        sb.Append($"import {ModuleName} from '{ModulePath}';");
    }
}