using Antlr4.Runtime.Tree;
using CrossBind.Compiler.Symbol;
using CrossBind.Compiler.Typing;
using CrossBind.Engine.BaseModels;
using CrossBind.Engine.Types;
using static HaibtParser;

namespace CrossBind.Compiler.Visitors.Properties;

public class PropertyVisitor: HaibtBaseVisitor<PropModel>
{
    private readonly TypeManager _manager;
    private readonly SymbolTable _scope;

    public PropertyVisitor(TypeManager manager, SymbolTable scope)
    {
        _manager = manager;
        _scope = scope;
    }

    private TypeModel FromTypeNotation(Type_valContext context)
    {
        ITerminalNode? primitive = context.PRIMIRIVE_TYPE();
        if (primitive is null) return Primitive.String();
        return primitive.Symbol.Text switch
        {
            "string" => Primitive.String(),
            "number" => Primitive.Number(),
            "bool" => Primitive.Bool(),
            _ => Primitive.String()
        };
    }

    private TypeModel FromConstValue(CONST_VALUEContext value)
    {
        if (value.NUMBER() is not null)
        {
            return Primitive.Number();
        }
        
        if (value.STRING() is not null)
        {
            return Primitive.String();
        }
        
        if (value.BOOLEAN() is not null)
        {
            return Primitive.Bool();
        }
        
        return Primitive.String();
    }

    private TypeModel FromIdentifier(string identifier)
    {
        var symbol = _scope.Symbols.FirstOrDefault(s => s.Identifier == identifier);
        return symbol?.Type ?? Primitive.String();
    }

    private TypeModel InferType(ValueContext context)
    {
        CONST_VALUEContext? value = context.cONST_VALUE();
        if (value is not null)
        {
            return FromConstValue(value);
        }

        ITerminalNode? identifier = context.IDENTIFIER();
        if (identifier is not null)
        {
            return FromIdentifier(identifier.GetText());
        }

        return Primitive.String();
    }
    
    public override PropModel VisitAutoInit(AutoInitContext context)
    {
        string propName = context.IDENTIFIER().GetText();
        Type_valContext? typeValue = context.type_val();
        ValueContext? value = context.value();
        TypeModel type = typeValue is not null 
            ? FromTypeNotation(typeValue)
            : InferType(value);
        _manager.RegisterType(type);
        SymbolEntry? entry = _scope.Symbols.FirstOrDefault(e => e.Identifier == propName);
        if (entry is null)
        {
            entry = new SymbolEntry(type)
            {
                Identifier = propName,
            };
            entry.Usages.Add(context.IDENTIFIER().Symbol);
        }
        _scope.Symbols.Add(entry);
        return new PropModel(propName, type);
    }
}