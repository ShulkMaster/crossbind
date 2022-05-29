using CrossBind.Engine.BaseModels;

namespace CrossBind.Compiler.Visitors;

public class ImportVisitor: HaibtBaseVisitor<ImportModel>
{
    public override ImportModel VisitImportStatement(HaibtParser.ImportStatementContext context)
    {
        var path = context.STRING()?.GetText() ?? "";
        var identifiers = context.IDENTIFIER();
        var symbols = new string[identifiers.Length];
        for (var i = 0; i < symbols.Length; i++)
        {
            symbols[i] = identifiers[i].GetText();
        }
        return new ImportModel(path, symbols);
    }
}