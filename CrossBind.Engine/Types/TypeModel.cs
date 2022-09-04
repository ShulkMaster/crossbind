namespace CrossBind.Engine.Types;

public abstract record TypeModel(
    string Name,
    string FQDN,
    bool Nullable
);