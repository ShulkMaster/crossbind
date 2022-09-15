using CrossBind.Compiler.Parser;
using CrossBind.Engine.BaseModels;

namespace CrossBind.Compiler.Visitors;

public class ImportVisitor: HaibtBaseVisitor<ImportModel>
{
    public override ImportModel VisitImportStatement(Haibt.ImportStatementContext context)
    {
        string path = context.StringLiteral()?.GetText() ?? "";
        var identifiers = context.IDENTIFIER();
        string[] symbols = new string[identifiers.Length];
        for (int i = 0; i < symbols.Length; i++)
        {
            symbols[i] = identifiers[i].GetText();
        }
        return new ImportModel(path, symbols);
    }
}