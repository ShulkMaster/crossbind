namespace CrossBind.Model;

public class ImportModel
{
    public readonly string Path;
    public readonly IEnumerable<string> Simbols;

    public ImportModel(string path, IEnumerable<string> simbols)
    {
        Simbols = simbols;
        Path = path;
    }
}