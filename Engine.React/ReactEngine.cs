using System.Text;
using CrossBind.Engine;
using CrossBind.Engine.BaseModels;
using CrossBind.Engine.ComponentModels;
using CrossBind.Engine.Generated;
using CrossBind.Engine.StyleModel;
using CrossBind.Engine.Types;
using Engine.React.Component;
using Engine.React.Extensions;
using Engine.React.Generation;
using Engine.React.Import;

namespace Engine.React;

public class ReactEngine : IEngine
{
    public string Name => "React Engine Official DEBUG";

    private static PropModel MakeProp(ComponentVariant variantStyle)
    {
        string varName = variantStyle.Name;
        VariantStyle? defaultVar = variantStyle.Styles.FirstOrDefault(s => s.Default);
        bool isNull = defaultVar is null;
        var varType = new UnionType(varName, "", !isNull);
        foreach (string key in variantStyle.GetKeys())
        {
            var literal = new StringLiteralType($"'{key}'", $"{varName}.{key}");
            varType.TypeModels.Add(literal);
        }

        return new ConstPropModel(varName, varType, defaultVar?.ValueKey ?? string.Empty);
    }

    private static ReactComponent CompileComponent(ComponentModel model)
    {
        var props = model.Body.Props;
        var variantProps = model.Body.Variants.Select(MakeProp);
        props.AddRange(variantProps);
        foreach (PropModel propModel in props)
        {
            Console.WriteLine(propModel.Name);
        }

        return new ReactComponent
        {
            Name = model.Name,
            Model = model,
            Properties = props,
            ComponentType = ComponentType.Functional
        };
    }

    private static void CompileComponentStyle(ComponentModel model, StringBuilder sb)
    {
        string prefix = model.Name;
        var styles = model.Body.BaseStyles;
        if (styles.Any())
        {
            sb.AppendLine($".{prefix} {{");
            foreach (ComponentStyle style in styles)
            {
                sb.Append(' ', 2);
                sb.AppendLine(style.StringValue);
            }

            sb.AppendLine("}");
        }

        var variants = model.Body.Variants;
        foreach (ComponentVariant variant in variants)
        {
            foreach (VariantStyle style in variant.Styles)
            {
                sb.AppendLine($"\n.{prefix}.{style.ValueKey} {{");
                foreach (ComponentStyle variantStyle in style.VariantStyles)
                {
                    sb.Append(' ', 2);
                    sb.Append(variantStyle.StringValue);
                    sb.Append('\n');
                }

                sb.Pop();
                sb.AppendLine("}");
            }

            sb.Append('\n');
        }
    }

    private static string CompileStyle(UnitModel unit)
    {
        var sb = new StringBuilder();
        var components = unit.Models;
        foreach (BindModel component in components)
        {
            switch (component.Type)
            {
                case ModelType.Component:
                    CompileComponentStyle((ComponentModel)component, sb);
                    break;
                case ModelType.ShareLib:
                    throw new NotImplementedException("No shared libs implementation");
                case ModelType.Asset:
                    throw new NotImplementedException("No assets implementation");
                default:
                    throw new ArgumentOutOfRangeException("Type", "Not in range");
            }
        }

        return sb.ToString();
    }

    public SourceFile[] CompileUnit(UnitModel unit)
    {
        string baseName = Path.GetFileName(unit.FilePath);
        int dotIndex = baseName.LastIndexOf('.');
        string fileName = baseName[..dotIndex];
        var module = new ReactModule(unit.FilePath.Replace(Path.PathSeparator, '.'));
        module.ResolveImports(unit);
        var files = new List<SourceFile>();

        string styleSource = CompileStyle(unit);
        module.StyleImports.Add(new StyleImport($"./{fileName}.css"));
        files.Add(new SourceFile(fileName, "css")
        {
            SourceCode = styleSource,
            SourceName = unit.FilePath
        });

        foreach (BindModel model in unit.Models)
        {
            switch (model)
            {
                case ComponentModel cModel:
                {
                    ReactComponent comp = CompileComponent(cModel);
                    module.Components.Add(comp);
                    break;
                }
            }
        }

        var sb = new StringBuilder();
        ComponentWriter cw = new(sb);

        cw.WriteSource(module);

        files.Add(new SourceFile(fileName, "tsx")
        {
            SourceCode = sb.ToString(),
            SourceName = unit.FilePath,
        });
        return files.ToArray();
    }
}