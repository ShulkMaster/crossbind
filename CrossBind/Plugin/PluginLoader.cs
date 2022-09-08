using System.Reflection;
using System.Runtime.Loader;
using CrossBind.Engine;

namespace CrossBind.Plugin;

public class PluginLoader
{
    private const string BasePath = "plugins";
    private readonly string _context;

    public PluginLoader()
    {
        string assemblyDir = AppContext.BaseDirectory;
        _context = new FileInfo(assemblyDir).DirectoryName ?? BasePath;
    }

    private static IEnumerable<string> GetDllDirs()
    {
        string assemblyDir = AppContext.BaseDirectory;
        string info = new FileInfo(assemblyDir).DirectoryName ?? "";
        return Directory.EnumerateFiles(Path.Combine(info, BasePath), "*.dll");
    }

    private static ICrossPlugin? LoadPlugin(Assembly assembly)
    {
        Type? pluginType = assembly.GetExportedTypes()
            .FirstOrDefault(t => typeof(ICrossPlugin).IsAssignableFrom(t));

        if (pluginType is null)
        {
            return null;
        }

        return Activator.CreateInstance(pluginType) as ICrossPlugin;
    }

    private static ICrossPlugin? LoadPluginByTarget(Assembly assembly, string target)
    {
        ICrossPlugin? activated = LoadPlugin(assembly);
        return string.Equals(activated?.Target, target, StringComparison.InvariantCultureIgnoreCase)
            ? activated
            : null;
    }

    private static ICrossPlugin? LoadPluginById(Assembly assembly, string id)
    {
        ICrossPlugin? activated = LoadPlugin(assembly);
        return string.Equals(activated?.PluginId, id, StringComparison.InvariantCultureIgnoreCase)
            ? activated
            : null;
    }

    private static Assembly LoadAssembly(string relativePath, AssemblyLoadContext pContext)
    {
        string absolute = Path.GetFullPath(relativePath);
        return pContext.LoadFromAssemblyPath(absolute);
    }

    public List<ICrossPlugin> FindEnginesForTarget(string target)
    {
        var list = new List<ICrossPlugin>();
        var dlls = GetDllDirs();
        foreach (string dll in dlls)
        {
            try
            {
                var pContext = new PluginContext(_context);
                Assembly pluginAssembly = LoadAssembly(dll, pContext);
                ICrossPlugin? plugin = LoadPluginByTarget(pluginAssembly, target);
                if (plugin is not null)
                {
                    list.Add(plugin);
                }
                else
                {
                    pContext.Unload();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

        return list;
    }

    public ICrossPlugin? FindEngineWithId(string id)
    {
        var dlls = GetDllDirs();
        foreach (string dll in dlls)
        {
            try
            {
                var pContext = new PluginContext(_context);
                Assembly pluginAssembly = LoadAssembly(dll, pContext);
                ICrossPlugin? plugin = LoadPluginById(pluginAssembly, id);
                if (plugin is not null)
                {
                    return plugin;
                }

                pContext.Unload();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

        return null;
    }
}