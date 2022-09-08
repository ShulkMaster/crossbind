using System.Text;

namespace Engine.React.Import;

public abstract class ReactImport
{
    public string ModulePath { get; init; }

    protected ReactImport(string path)
    {
        ModulePath = path;
    }

    public abstract void WriteStatement(StringBuilder sb, int indent);
}