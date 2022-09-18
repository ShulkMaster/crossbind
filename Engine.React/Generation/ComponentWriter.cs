using System.Text;
using CrossBind.Engine.BaseModels;
using CrossBind.Engine.ComponentModels;
using CrossBind.Engine.Markup;
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
        _sb.AppendLine("// Todo fill the code");
        WriteMarkup(component.Model);
        _sb.Append("};");
    }

    private void WriteMarkup(ComponentModel component)
    {
        var variants = component.Body.Variants;
        Tag root = component.Body.Html;
        if (root is NoTag) return;

        _sb.AppendLine("  return (");
        WriteMarkup(component.Body.Html, variants, 2);
        _sb.AppendLine("  );");
    }

    private void WriteMarkup(Tag tag, List<ComponentVariant> vars, int level)
    {
        switch (tag)
        {
            case NoTag: return;
            case NativeTag navTag:
            {
                _sb.Append(' ', level * 2);
                _sb.Append($"<{navTag.Name}");
                WriteAttribs(tag);
                if (navTag.Content.Any())
                {
                    _sb.AppendLine(">");
                    foreach (HtmlContent content in navTag.Content)
                    {
                        WriteContent(content, vars, level + 1);
                    }
                    _sb.Append(' ', level * 2);
                    _sb.AppendLine($"</{navTag.Name}>");
                }
                else
                {
                    _sb.AppendLine("/>");
                }
                return;
            }
            case ComponentTag cTag:
            {
                _sb.Append(' ', level * 2);
                _sb.Append($"<{cTag.Name}");
                WriteAttribs(tag);
                if (cTag.Content.Any())
                {
                    _sb.AppendLine(">");
                    foreach (HtmlContent content in cTag.Content)
                    {
                        WriteContent(content, vars,level + 1);
                    }
                    _sb.Append(' ', level * 2);
                    _sb.AppendLine($"</{cTag.Name}>");
                }
                else
                {
                    _sb.AppendLine("/>");
                }
                return;
                
            }
        }
    }

    private void WriteContent(HtmlContent content, List<ComponentVariant> vars, int level)
    {
        switch (content)
        {
            case HtmlText text:
            {
                _sb.Append(' ', level * 2);
                _sb.AppendLine(text.GetCuratedString());
                return;
            }
            case Tag tag:
            {
                WriteMarkup(tag, vars, level);
                return;
            }
        }
    }

    private void WriteAttribs(Tag tag)
    {
        foreach (PropModel tagAttribute in tag.Attributes)
        {
            _sb.Append($" {tagAttribute.Name}={{{tagAttribute.Type.Name}}}");
        }
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