using System.Text;
using CrossBind.Engine.BaseModels;
using CrossBind.Engine.ComponentModels;
using CrossBind.Engine.Types;
using Engine.React.Component;
using Engine.React.Extensions;
using Engine.React.Import;
using System.Globalization;

namespace Engine.React.Generation;

public class ComponentWriter
{
    private readonly StringBuilder _sb;

    public ComponentWriter(StringBuilder sb)
    {
        _sb = sb;
    }

    public void WriteSource(ReactModule module)
    {
        WriteImports(module.NamedImports);
        WriteImports(module.DefaultImports);
        WriteImports(module.StyleImports);
        _sb.AppendLine();
        module.Components.ForEach(WriteComponent);
    }

    private void WriteImports(IEnumerable<NamedImport> imports)
    {
        foreach (NamedImport import in imports)
        {
            _sb.Append("import {");

            foreach (string symbol in import.Symbols)
            {
                _sb.Append($" {symbol},");
            }

            _sb.Remove(_sb.Length - 1, 1);
            _sb.Append($"}} from '{import.ModulePath}';\n");
        }
    }

    private void WriteImports(IEnumerable<DefaultImport> imports)
    {
        foreach (DefaultImport import in imports)
        {
            _sb.Append($"import {import.ModuleName}");

            if (import.Symbols.Any())
            {
                _sb.Append(", {");
                foreach (string symbol in import.Symbols)
                {
                    _sb.Append($" {symbol},");
                }

                _sb.Pop();
                _sb.Append('}');
            }

            _sb.AppendLine($" from '{import.ModulePath}';");
        }
    }

    private void WriteImports(IEnumerable<StyleImport> imports)
    {
        foreach (StyleImport import in imports)
        {
            _sb.AppendLine($"import '{import.ModulePath}';");
        }
    }

    private void WriteProps(IEnumerable<PropModel> props)
    {
        foreach (PropModel prop in props)
        {
            TypeModel type = prop.Type;
            string nullable = type.Nullable ? "?" : "";
            switch (prop)
            {
                case ConstPropModel cModel:
                {
                    _sb.AppendLine(
                        $"  const {cModel.Name}: {type.Name}{nullable} = p.{cModel.Name} || {cModel.ConstValue};");
                    continue;
                }
            }
        }
    }

    private void WriteComponent(ReactComponent component)
    {
        string name = component.Name;
        WriteComponentPropType(component);
        _sb.Append($"export const {name} = (p: {name}Props) => {{\n");
        WriteProps(component.Properties);
        string tag = DomReactTypes.GetComponentTag(component.Model.Extends);
        _sb.AppendLine("// Todo fill the code");
        _sb.Append($"  <{tag} className=");
        var variants = component.Model.Body.Variants;
        if (variants.Any())
        {
            _sb.Append($"{{`{name}");
            foreach (ComponentVariant variant in variants)
            {
                _sb.Append($" ${{{variant.Name}}}");
            }

            _sb.Append("`}");
        }

        _sb.AppendLine(" {...p} />;\n};");
    }

    private void WriteComponentPropType(ReactComponent component)
    {
        string typeName = component.Name
            .First()
            .ToString()
            .ToUpper();
        typeName += component.Name[1..];
        _sb.AppendLine($"export type {typeName}Props = {{");
        foreach (PropModel prop in component.Properties)
        {
            string nullable = prop.Type.Nullable ? "?" : "";
            _sb.AppendLine($"  {prop.Name}{nullable}: {prop.Type.Name};");
        }
        _sb.AppendLine("}\n");
    }
}