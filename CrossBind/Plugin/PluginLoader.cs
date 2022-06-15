using System.Reflection;
using CrossBind.Compiler.exceptions;
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

    private static IEngine LoadEngine(Assembly assembly)
    {
        var engineType = assembly.GetTypes()
            .FirstOrDefault(t => typeof(IEngine).IsAssignableFrom(t));

        if (engineType is null)
        {
            string availableTypes = string.Join("\n", assembly.GetTypes().Select(t => t.FullName));
            Console.WriteLine(availableTypes);
            throw new InvalidAssemblyException(
                $"Can't find any type which implements {nameof(IEngine)} in {assembly} from {assembly.Location}");
        }

        var result = Activator.CreateInstance(engineType) as IEngine;
        return result!;
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
                list.Add(LoadEngine(pluginAssembly));
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }

        return list;
    }
}