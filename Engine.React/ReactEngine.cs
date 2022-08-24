﻿using System.Text;
using CrossBind.Engine;
using CrossBind.Engine.BaseModels;
using CrossBind.Engine.ComponentModels;
using CrossBind.Engine.Generated;
using CrossBind.Engine.StyleModel;

namespace Engine.React;

public class ReactEngine : IEngine
{
    public string PluginId => "React.Official";
    public string PluginName => "React Engine Official";
    public int MajorVersion => 0;
    public int MinorVersion => 1;
    public int PathVersion => 0;
    public EngineTarget Target => EngineTarget.React;

    private static SourceFile CompileComponent(ComponentModel model, StringBuilder sb, string source)
    {
        sb.Append(DomReactTypes.GetReactImports(model.Extends));
        (string typeName, string typeDefinition) = DomReactTypes.GetDomType(model.Extends);
        Console.WriteLine(typeName, typeDefinition);
        sb.Append(typeDefinition);
        // todo code that gets every prop in the component
        sb.Append($"\nexport type {model.Name}Props = {{\n");

        foreach (var variant in model.Body.Variants)
        {
            sb.Append($"  {variant.Name}?: ");
            foreach (VariantStyle style in variant.Styles)
            {
                sb.Append($"'{style.ValueKey}' |");
            }
            sb.Replace('|', ';', sb.Length - 1, 1);
            sb.Append('\n');
        }

        if (typeName != string.Empty)
        {
            sb.Append($"}} & {typeName};\n");
        }
        else
        {
            sb.Append("}\n");
        }

        sb.Append($"\nexport const {model.Name} = (props : {model.Name}Props");
        string tag = DomReactTypes.GetComponentTag(model.Extends);
        if (tag == DomReactTypes.ReactInputTag)
        {
            tag += " type='text'";
        }

        ComponentVariant variantM = model.Body.Variants.First();
        
        sb.Append($") => {{\n");
        var x = model.Body.Variants.First();
        sb.Append($"  const {variantM.Name} = props.{x.Name} || '{x.Styles.First().ValueKey}';\n");
        sb.Append($"// Todo fill the code\nreturn <{tag} className={{`{model.Name} ${{{variantM.Name}}}`}} {{...props}} />\n}};\n");

        string styles = CompileStyle(model.Body.BaseStyles, model.Name);
        string vars = CompileVariant(model.Body.Variants.First(), model.Name);
        return new SourceFile(source, "css")
        {
            SourceCode = $"{styles}\n{vars}",
            SourceName = model.ModuleId,
        };
    }

    private static SourceFile CompileModel(BindModel model, StringBuilder sb, string src)
    {
        switch (model)
        {
            case ComponentModel cModel:
            {
                return CompileComponent(cModel, sb, src);
            }
        }

        return new SourceFile("", "");
    }

    private static string CompileStyle(IEnumerable<ComponentStyle> styles, string baseName)
    {
        var sb = new StringBuilder();
        sb.Append($".{baseName} {{\n");
        foreach (var style in styles)
        {
            if (style.Key == "background-color")
            {
                sb.AppendFormat("  background-color: {0};\n", style.StringValue);
                continue;
            }

            sb.AppendFormat("  {0}", style.StringValue);
        }
        sb.Append("}\n");
        return sb.ToString();
    }

    private static string CompileVariant(ComponentVariant variant, string baseName)
    {
        var sb = new StringBuilder();
        foreach (var value in variant.Styles)
        {
            sb.Append($".{baseName}.{value.ValueKey} {{\n");
            foreach (var style in value.VariantStyles)
            {
                if (style.Key == "background-color")
                {
                    sb.AppendFormat("  background-color: {0};\n", style.StringValue);
                    continue;
                }

                sb.AppendFormat("  {0}", style.StringValue);
            }
            sb.Append("}\n");
        }
        
        return sb.ToString();
    }

    public SourceFile[] CompileUnit(UnitModel unit, bool production)
    {
        var sb = new StringBuilder();
        string baseName = Path.GetFileName(unit.FilePath);
        int dotIndex = baseName.LastIndexOf('.');
        string fileName = baseName[..dotIndex];
        foreach (var module in unit.Modules)
        {
            sb.Append("import { ");
            sb.AppendJoin(',', module.Simbols);
            sb.Append(" } from ");
            sb.Append(module.Path);
            sb.Append(";\n");
        }

        sb.Append("import './");
        sb.Append(fileName);
        sb.Append(".css';\n\r");

        var files = new List<SourceFile>();
        foreach (var model in unit.Models)
        {
            var source = CompileModel(model, sb, fileName);
            files.Add(source);
        }

        files.Add(new SourceFile(fileName, "tsx")
        {
            SourceCode = sb.ToString(),
            SourceName = unit.FilePath,
        });
        return files.ToArray();
    }
}