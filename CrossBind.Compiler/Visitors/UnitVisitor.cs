using CrossBind.Compiler.Visitors.Component;
using CrossBind.Model;

namespace CrossBind.Compiler.Visitors;

public class UnitVisitor: HaibtBaseVisitor<UnitModel>
{
    public string FilePath { get; set; } = string.Empty;
    
    public override UnitModel VisitTranslationUnit(HaibtParser.TranslationUnitContext context)
    {
        var imports = context.importStatement();
        var importVisitor = new ImportVisitor();
        var componentVisitor = new ComponentVisitor();
        var modules = imports.Select(importVisitor.VisitImportStatement);
        var comps = context.compDeclaration();
        var libs = context.libFile();
        var components = comps.Select(componentVisitor.VisitCompDeclaration);
        var models = new BindModel[comps.Length + libs.Length];
        var index = 0;
        foreach (var component in components)
        {
            models[index] = component;
            index++;
        }

        for (int i = index; i < models.Length; i++)
        {
            models[i] = new SharedLib();
        }
        
        return new UnitModel("#my_hash", FilePath, modules, models);
    }
}