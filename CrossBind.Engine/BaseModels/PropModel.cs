using System.Linq.Expressions;
using CrossBind.Engine.Types;

namespace CrossBind.Engine.BaseModels;

public record PropModel(string Name, TypeModel Type);

public sealed record ConstPropModel(
    string Name,
    TypeModel Type,
    string ConstValue
) : PropModel(Name, Type);

public sealed record AssignPropModel(
    string Name,
    TypeModel Type,
    string Identifier
) : PropModel(Name, Type);

public sealed record ExpressionPropModel(
    string Name,
    TypeModel Type,
    Expression Expression
) : PropModel(Name, Type);
