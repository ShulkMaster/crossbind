namespace CrossBind.Engine.Generated;

public class SourceFile
{
    public string SourceName { get; init; } = string.Empty;
    public string FileName { get; }
    public string SourceCode { get; init; } = string.Empty;
    public string Extension { get; }

    public SourceFile(string fileName, string extension)
    {
        FileName = fileName;
        Extension = extension;
    }
}