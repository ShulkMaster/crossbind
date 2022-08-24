namespace CrossBind.Compiler.Test.Samples;

public static class VariantSample
{
    public const string VarName = "style";
    
    public const string VariantDeclaration = $"variant {VarName};";

    public const string VariantInitialization = @$"
{VarName} primary {{
    border: 2 solid #DDD;
    backgroundColor: #FFB;
}}";
}