using System.Linq.Expressions;
using CrossBind.Engine.Types;

namespace CrossBind.Engine.BaseModels;

public record PropModel(string Name, TypeModel Type);

public record ConstPropModel(
    string Name,
    TypeModel Type,
    string ConstValue
) : PropModel(Name, Type);

public record AssignPropModel(
    string Name,
    TypeModel Type,
    string Identifier
) : PropModel(Name, Type);

public record ExpressionPropModel(
    string Name,
    TypeModel Type,
    Expression Expression
) : PropModel(Name, Type);
