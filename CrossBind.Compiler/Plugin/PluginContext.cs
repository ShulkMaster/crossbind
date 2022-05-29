using System.Reflection;
using System.Runtime.Loader;
using CrossBind.Engine;

namespace CrossBind.Compiler.Plugin;

internal class PluginContext: AssemblyLoadContext
{
    private readonly AssemblyDependencyResolver _resolver;
    
    public PluginContext(string pluginPath)
    {
        _resolver = new AssemblyDependencyResolver(pluginPath);
    }
    
    protected override Assembly? Load(AssemblyName assemblyName)
    {
        if (assemblyName.FullName == typeof(IEngine).Assembly.FullName)
        {
            return null;
        }
        string? assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
        return assemblyPath is not null ? LoadFromAssemblyPath(assemblyPath) : null;
    }
    
    protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
    {
        string? libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
        return libraryPath is not null ? LoadUnmanagedDllFromPath(libraryPath) : IntPtr.Zero;
    }
}