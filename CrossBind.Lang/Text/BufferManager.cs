using System.Collections.Concurrent;
using Antlr4.Runtime;

namespace CrossBind.Lang.Text;

public class BufferManager
{
    private readonly ConcurrentDictionary<string, AntlrInputStream> _buffers = new();

    public void UpdateBuffer(string documentPath, AntlrInputStream buffer)
    {
        _buffers.AddOrUpdate(documentPath, buffer, (k, v) => buffer);
    }

    public AntlrInputStream? GetBuffer(string documentPath)
    {
        return _buffers.TryGetValue(documentPath, out var buffer) ? buffer : null;
    }
}