namespace CrossBind.Engine.Types;

public record Primitive: TypeModel
{
    public Primitives PrimitiveType { get; init; }

    private Primitive(string name, string primitive, bool nullable) : base(name, primitive, nullable){}

    public static Primitive String(bool nullable)
    {
        return new Primitive(nameof(String).ToLower(), "CrossBind.String", nullable)
        {
            PrimitiveType = Primitives.String,
        };
    }
    
    public static Primitive Number(bool nullable)
    {
        return new Primitive(nameof(Number).ToLower(), "CrossBind.Number", nullable)
        {
            PrimitiveType = Primitives.Number,
        };
    }
    
    public static Primitive Bool(bool nullable)
    {
        return new Primitive(nameof(Bool).ToLower(), "CrossBind.Bool", nullable)
        {
            PrimitiveType = Primitives.Bool,
        };
    }
}