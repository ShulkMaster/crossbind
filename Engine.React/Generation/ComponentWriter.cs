using System.Text;
using CrossBind.Engine.BaseModels;
using CrossBind.Engine.ComponentModels;
using CrossBind.Engine.Types;
using Engine.React.Component;

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
        var imports = module.Imports.OrderBy(m => m.Simbols);
        WriteImports(imports);
        module.Components.ForEach(WriteComponent);
    }

    private void WriteImports(IEnumerable<ImportModel> imports)
    {
        foreach (ImportModel import in imports)
        {
            if (import.Path.EndsWith(".css"))
            {
                _sb.AppendLine($"import '{import.Path}';");
                continue;
            }

            _sb.Append("import {");
            if (import.Simbols.Any())
            {
                foreach (string symbol in import.Simbols)
                {
                    _sb.Append($" {symbol},");
                }

                _sb.Remove(_sb.Length - 1, 1);
            }

            _sb.Append($"}} from '{import.Path}';\n");
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
                    _sb.AppendLine($"  const {cModel.Name}: {cModel.Name}{nullable} = p.{cModel.Name} || {cModel.ConstValue};");
                    continue;
                }
            }
        }
    }

    private void WriteComponent(ReactComponent component)
    {
        string name = component.Name;
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

            _sb.Append("`}}");
        }

        _sb.AppendLine(" {...props} />;\n};");
    }
}