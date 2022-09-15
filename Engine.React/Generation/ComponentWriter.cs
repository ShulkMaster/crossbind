using System.Text;
using CrossBind.Engine.BaseModels;
using CrossBind.Engine.ComponentModels;
using CrossBind.Engine.Types;
using Engine.React.Component;
using Engine.React.Extensions;
using Engine.React.Import;

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

    private string GetConstValue(ConstPropModel constProp)
    {
        TypeModel type = constProp.Type;
        if (type is UnionType st)
        {
            foreach (TypeModel stTypeModel in st.TypeModels)
            {
                if (stTypeModel is StringLiteralType sl && sl.Name == constProp.ConstValue)
                {
                    return sl.Value;
                }
            }
        }

        return constProp.ConstValue;
    }

    private void WriteProps(IEnumerable<PropModel> props, string componentName)
    {
        foreach (PropModel prop in props)
        {
            TypeModel type = prop.Type;
            string typeNotation = type.Name;
            if (type is UnionType)
            {
                typeNotation = $"{componentName}Props['{prop.Name}']";
            }

            switch (prop)
            {
                case ConstPropModel cModel:
                {
                    bool optional = !string.IsNullOrEmpty(cModel.ConstValue);
                    _sb.Append($"  const {cModel.Name}: {typeNotation} = p.{cModel.Name}");
                    if (optional)
                    {
                        _sb.AppendLine($" || {GetConstValue(cModel)};");
                    }
                    else
                    {
                        _sb.Append(";\n");
                    }

                    continue;
                }
            }
        }
    }

    private void WriteComponent(ReactComponent component)
    {
        string name = component.Name.Capitalize();
        WriteComponentPropType(component);
        _sb.Append($"export const {name} = (p: {name}Props) => {{\n");
        WriteProps(component.Properties, name);
        const string tag = "button";
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
        string typeName = component.Name.Capitalize();
        _sb.AppendLine($"export type {typeName}Props = {{");
        foreach (PropModel prop in component.Properties)
        {
            bool isOptional = prop.Type.Nullable;
            if (prop is ConstPropModel constProp)
            {
                isOptional = !string.IsNullOrEmpty(constProp.ConstValue);
            }

            string nullable = isOptional ? "?" : "";
            if (prop.Type is UnionType union)
            {
                _sb.Append($"  {prop.Name}{nullable}:");
                foreach (TypeModel type in union.TypeModels)
                {
                    if (type is StringLiteralType sl)
                    {
                        _sb.Append($" {sl.Value} |");
                        continue;
                    }
                    _sb.Append($" {type.Name} |");
                }

                _sb.Pop();
                _sb.AppendLine(";");
                continue;
            }

            _sb.AppendLine($"  {prop.Name}{nullable}: {prop.Type.Name};");
        }

        _sb.AppendLine("}\n");
    }
}