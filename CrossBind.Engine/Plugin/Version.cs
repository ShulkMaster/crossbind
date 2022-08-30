namespace CrossBind.Engine.Plugin;

public record Version(int Major, int Minor, int Patch)
{
    public override string ToString()
    {
        return $"v{Major}.{Minor}.{Patch}";
    }

    public static bool operator <(Version a, Version b)
    {
        return a.Major < b.Major || a.Minor < b.Minor;
    }
    
    public static bool operator >(Version a, Version b)
    {
        return a.Major > b.Major || a.Minor > b.Minor;
    }
}