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

    private TypeModel FromTypeNotation(Type_valContext context, bool nullable)
    {
        ITerminalNode? primitive = context.PRIMIRIVE_TYPE();
        if (primitive is null) return Primitive.String(false);
        return primitive.Symbol.Text switch
        {
            "string" => Primitive.String(nullable),
            "number" => Primitive.Number(nullable),
            "bool" => Primitive.Bool(nullable),
            _ => Primitive.String(nullable)
        };
    }

    private TypeModel FromConstValue(Const_valueContext value)
    {
        if (value.NUMBER() is not null)
        {
            return Primitive.Number(false);
        }
        
        if (value.STRING() is not null)
        {
            return Primitive.String(false);
        }
        
        if (value.BOOLEAN() is not null)
        {
            return Primitive.Bool(false);
        }
        
        return Primitive.String(false);
    }

    private TypeModel FromIdentifier(string identifier)
    {
        var symbol = _scope.Symbols.FirstOrDefault(s => s.Identifier == identifier);
        return symbol?.Type ?? Primitive.String(false);
    }

    private TypeModel InferType(ValueContext context)
    {
        Const_valueContext? value = context.const_value();
        if (value is not null)
        {
            return FromConstValue(value);
        }

        ITerminalNode? identifier = context.IDENTIFIER();
        if (identifier is not null)
        {
            return FromIdentifier(identifier.GetText());
        }

        return Primitive.String(false);
    }
    
    public override PropModel VisitAutoInit(AutoInitContext context)
    {
        string propName = context.IDENTIFIER().GetText();
        Type_valContext? typeValue = context.type_val();
        ValueContext? value = context.value();
        bool nullable = context.QUEST() is not null;
            TypeModel type = typeValue is not null 
            ? FromTypeNotation(typeValue, nullable)
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