using CrossBind.Parser.Implementation;
using CrossBind.Compiler.Visitors.Component;
using CrossBind.Engine.BaseModels;
using CrossBind.Engine.ComponentModels;

namespace CrossBind.Compiler.Visitors;

public class UnitVisitor: HaibtBaseVisitor<UnitModel>
{
    public string FilePath { get; set; } = string.Empty;
    
    public override UnitModel VisitTranslationUnit(Haibt.TranslationUnitContext context)
    {
        var imports = context.importStatement();
        var importVisitor = new ImportVisitor();
        var componentVisitor = new ComponentVisitor();
        var modules = imports.Select(importVisitor.VisitImportStatement).ToList();
        var comps = context.compDeclaration();
        var cssRules = context.css_rule();
        var components = comps.Select(componentVisitor.VisitCompDeclaration);
        var models = new BindModel[comps.Length + cssRules.Length];
        int index = 0;
        foreach (ComponentModel component in components)
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