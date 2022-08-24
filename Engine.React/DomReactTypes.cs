using CrossBind.Engine.ComponentModels;

namespace Engine.React;

public static class DomReactTypes
{
    private const string ReactButton =
        $"type {nameof(ReactButton)} = DetailedHTMLProps<ButtonHTMLAttributes<HTMLButtonElement>,HTMLButtonElement>;";

    public const string ReactSvg = $"type {nameof(ReactSvg)} = Omit<SVGProps<SVGSVGElement>, 'viewBox' | 'xmlns'>;";

    private const string ReactInput =
        $"type {nameof(ReactInput)} = DetailedHTMLProps<InputHTMLAttributes<HTMLInputElement>,HTMLInputElement>;";

    public const string ReactInputTag = "input";

    public static (string typeName, string typeDefinition) GetDomType(Extendable extend)
    {
        switch (extend)
        {
            case Extendable.Button:
                return (nameof(ReactButton), ReactButton);
            case Extendable.Select:
                break;
            case Extendable.TextBox:
                return (nameof(ReactInput), ReactInput);
            case Extendable.Component:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(extend), extend, null);
        }

        return (string.Empty, string.Empty);
    }

    public static string GetComponentTag(Extendable extend)
    {
        return extend switch
        {
            Extendable.Button => "button",
            Extendable.Select => "select",
            Extendable.TextBox => ReactInputTag,
            Extendable.Component => string.Empty,
            _ => throw new ArgumentOutOfRangeException(nameof(extend), extend, null)
        };
    }

    public static string GetReactImports(Extendable extend)
    {
        const string startImport = "import React";
        const string endImport = "from 'react';\n";
        const string attribImport = ", { DetailedHTMLProps, ";
        switch (extend)
        {
            case Extendable.Button:
                return startImport + attribImport + "ButtonHTMLAttributes } " + endImport;
            case Extendable.Select:
                break;
            case Extendable.TextBox:
                break;
            case Extendable.Component:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(extend), extend, null);
        }

        return string.Empty;
    }
}