namespace Engine.React.Import;

public sealed class ModuleImport
{
    private readonly SortedSet<ReactImport> _imports = new();

    public IEnumerable<ReactImport> Imports => _imports;

    public bool AddModule(ReactImport import)
    {
        return _imports.Add(import);
    }

    public bool AddSymbolToModule(string moduleId, string symbol)
    {
        ReactImport? import = _imports.FirstOrDefault(m => m.ModulePath == moduleId);
        if (import is not NamedImport namedImport) return false;
        return namedImport.Symbols.Add(symbol);
    }
}