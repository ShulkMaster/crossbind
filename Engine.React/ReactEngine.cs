using System.Text;
using CrossBind.Engine;
using CrossBind.Engine.BaseModels;
using CrossBind.Engine.ComponentModels;

namespace Engine.React;

public class ReactEngine : IEngine
{
    public string PluginId => "React.Official";
    public string PluginName => "React Engine Official";
    public int MajorVersion => 0;
    public int MinorVersion => 1;
    public int PathVersion => 0;
    public EngineTarget Target => EngineTarget.React;

    private static void CompileComponent(ComponentModel model, StringBuilder sb)
    {
        sb.Append(DomReactTypes.GetReactImports(model.Extends));
        (string typeName, string typeDefinition) = DomReactTypes.GetDomType(model.Extends);
        Console.WriteLine(typeName, typeDefinition);
        sb.Append(typeDefinition);
        // todo code that gets every prop in the component
        sb.Append($"\nexport type {model.Name}Props = {{\n");
        // todo write every props in type
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

        sb.Append($") => {{\n// Todo fill the code\n return <{tag} {{...props}} />\n}};\n");
    }
    
    private static void CompileModel(BindModel model, StringBuilder sb)
    {
        switch (model)
        {
            case ComponentModel cModel:
            {
                CompileComponent(cModel, sb);
                break;
            }
        }
    }

    public string CompileUnit(UnitModel unit, bool production)
    {
        var stringBuilder = new StringBuilder();
        foreach (var module in unit.Modules)
        {
            stringBuilder.Append("import { ");
            stringBuilder.AppendJoin(',', module.Simbols);
            stringBuilder.Append(" } from ");
            stringBuilder.Append(module.Path);
            stringBuilder.Append(";\n");
        }

        foreach (var model in unit.Models)
        {
            CompileModel(model, stringBuilder);
        }

        return stringBuilder.ToString();
    }
}