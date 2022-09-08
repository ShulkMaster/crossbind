namespace CrossBind.Compiler.Symbol;

public class SymbolTable
{
    public SymbolTable? UpperScope;
    public readonly SymbolTable? Root;

    public List<SymbolTable> Scopes { get; set; } = new();
    public List<SymbolEntry> Symbols { get; set; } = new();

    public SymbolTable(SymbolTable? scope, SymbolTable? root = null)
    {
        UpperScope = scope;
        Root = root;
    }
}