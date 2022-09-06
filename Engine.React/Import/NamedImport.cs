using System.Text;
using Engine.React.Extensions;

namespace Engine.React.Import;

public sealed class NamedImport: ReactImport
{
    public SortedSet<string> Symbols { get; } = new();

    public NamedImport(string path) : base(path)
    {
    }
    
    public override void WriteStatement(StringBuilder sb, int indent)
    {
        // -1 because of the comma separated imports but 1st does not have it
        int stringLenght = Symbols.Sum(s => s.Length) + Symbols.Count - 1;
        sb.Append(' ', indent);
        if (stringLenght < 90)
        {
            sb.Append("import {");
            foreach (string symbol in Symbols)
            {
                sb.Append($" {symbol},");
            }
            sb.Pop();
            sb.AppendLine($"}} from '{ModulePath}';");
            return;
        }
        
        sb.AppendLine("import {");
        
        foreach (string symbol in Symbols)
        {
            sb.Append(' ', indent + 2);
            sb.AppendLine($"{symbol},");
        }
        sb.Pop();
        sb.AppendLine($"}} from '{ModulePath}';");
    }

}