using Antlr4.Runtime;
using CrossBind.Engine.Types;

namespace CrossBind.Compiler.Symbol;

public class SymbolEntry
{
    public string Identifier { get; init; } = string.Empty;

    public TypeModel Type { get; init; }

    public List<IToken> Usages { get; set; } = new();
    
    public SymbolEntry(TypeModel type)
    {
        Type = type;
    }
    
}