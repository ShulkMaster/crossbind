namespace CrossBind.Engine.BaseModels;

public class ImportModel
{
    public readonly string Path;
    public readonly IEnumerable<string> Symbols;

    public ImportModel(string path, IEnumerable<string> symbols)
    {
        Symbols = symbols;
        Path = path;
    }
}