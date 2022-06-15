using System.Reflection;
using CrossBind.Engine;

namespace CrossBind.Plugin;

public class PluginLoader
{
    private const string BasePath = "plugins";
    private readonly PluginContext _context = new(BasePath);

    private static IEnumerable<string> GetDllDirs()
    {
        return Directory.EnumerateFiles(BasePath, "*.dll");
    }

    private static void LoadEngine(Assembly assembly, EngineTarget target, out IEngine? engine)
    {
        var engineType = assembly.GetTypes()
            .FirstOrDefault(t => typeof(IEngine).IsAssignableFrom(t));

        if (engineType is null)
        {
            engine = null;
            return;
        }

        engine = Activator.CreateInstance(engineType) as IEngine;
        if (engine?.Target == target)
        {
            return;
        }

        engine = null;
    }

    private Assembly LoadPlugin(string relativePath)
    {
        string absolute = Path.GetFullPath(relativePath);
        return _context.LoadFromAssemblyPath(absolute);
    }

    public List<IEngine> FindEnginesForTarget(EngineTarget target)
    {
        var list = new List<IEngine>();
        var dlls = GetDllDirs();
        foreach (string dll in dlls)
        {
            try
            {
                var pluginAssembly = LoadPlugin(dll);
                LoadEngine(pluginAssembly, target, out var e);
                if (e is not null)
                {
                    list.Add(e);
                }

            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }

        return list;
    }
}