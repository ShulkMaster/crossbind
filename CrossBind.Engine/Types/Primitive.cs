namespace CrossBind.Engine.Types;

public record Primitive: TypeModel
{
    public Primitives PrimitiveType { get; init; }

    private Primitive(string name, string primitive) : base(name, primitive){}

    public static Primitive String()
    {
        return new Primitive(nameof(String), "CrossBind.String")
        {
            PrimitiveType = Primitives.String,
        };
    }
    
    public static Primitive Number()
    {
        return new Primitive(nameof(Number), "CrossBind.Number")
        {
            PrimitiveType = Primitives.Number,
        };
    }
    
    public static Primitive Bool()
    {
        return new Primitive(nameof(Bool), "CrossBind.Bool")
        {
            PrimitiveType = Primitives.Bool,
        };
    }
}