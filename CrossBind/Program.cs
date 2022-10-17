using System.Reactive.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using CrossBind.Commands;
using CrossBind.Compiler;
using CrossBind.Config;
using CrossBind.Engine;
using CrossBind.Engine.Generated;
using CrossBind.Plugin;
using MediatR;
using Unit = LanguageExt.Unit;
using Microsoft.Extensions.DependencyInjection;

namespace CrossBind;

public static class Program
{
    private static readonly ManualResetEvent QuitEvent = new(false);
    private static readonly PluginLoader Loader = new();
    private static CrossConfig _conf = new();
    private static readonly CancellationTokenSource Source = new();

    static Program()
    {
        AppDomain.CurrentDomain.ProcessExit += Termination;
    }

    private static void Termination(object? sender, EventArgs eventArgs)
    {
        if (!Source.IsCancellationRequested)
        {
            Source.Cancel();
        }

        Source.Dispose();
    }

    public static async Task<int> Main(string[] args)
    {
        var commandParser = CommandLine.Parser.Default;
        var parserResult = commandParser.ParseArguments(args, typeof(Project), typeof(Compile));
        if (parserResult.Errors.Any())
        {
            return -1;
        }

        ServiceProvider sp = Setup.Init();
        var mediator = sp.GetService<IMediator>()!;
        var req = parserResult.Value as IRequest<int>;
        int result = await mediator.Send(req!);
        await sp.DisposeAsync();
        return result;
    }

    private static int LoadConfig(string configFile)
    {
        if (!configFile.EndsWith(".json"))
        {
            Console.Error.WriteLine("Config file must be a JSON file");
            return -1;
        }

        if (configFile == "crossbind.json" && !File.Exists(configFile))
        {
            return 0;
        }

        try
        {
            using Stream ss = File.OpenRead(configFile);
            var opts = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                AllowTrailingCommas = true,
                MaxDepth = 10,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            _conf = JsonSerializer.Deserialize<CrossConfig>(ss, opts) ?? _conf;
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
            Console.Error.WriteLine($"Error reading config file {configFile}");
            return -1;
        }
    }

    private static int ProjectCommand(Project command)
    {
        Console.Error.WriteLine("Not implemented");
        return -2;
    }

    private static bool IsFile(string path)
    {
        return Path.GetFullPath(path).Length != 0;
    }

    private static int CompileCommandProxy(Compile command)
    {
        int returnCode = LoadConfig(command.Config);
        if (!string.IsNullOrWhiteSpace(command.OutputDir))
        {
            _conf.OutDir = command.OutputDir;
        }

        if (returnCode != 0)
        {
            return returnCode;
        }

        ICrossPlugin? plugin = Loader.FindEngineWithId(command.PluginId);

        if (plugin is null)
        {
            Console.Error.WriteLine($"Unable to load plugin {command.PluginId}");
            return -1;
        }

        Console.WriteLine("Plugin loaded: {0} {1}", plugin.Name, plugin.Version);
        JsonNode? opts = null;
        _conf.Options?.TryGetPropertyValue(plugin.PluginId, out opts);
        if (opts is not null)
        {
            Console.WriteLine($"Using options {plugin.PluginId}:");
            Console.WriteLine(opts.ToJsonString());
        }

        IEngine engine = plugin.GetEngineInstance(false, opts?.AsObject());
        Directory.CreateDirectory(_conf.OutDir);
        if (!IsFile(command.Source)) return CompileCommand(engine, command.Source);

        if (!File.Exists(command.Source))
        {
            Console.Error.WriteLine("File: {0} does not exits", command.Source);
            return -1;
        }

        return command.Watch ? Watch(command, engine) : CompileCommand(command, engine);
    }

    private static int Watch(Compile command, IEngine engine)
    {
        string file = command.Source;
        Console.WriteLine("Watching file: {0}", file);
        var watcher = new FileSystemWatcher(Path.GetDirectoryName(file)!);
        string fName = Path.GetFileName(file);
        watcher.EnableRaisingEvents = true;
        watcher.Filter = fName;
        watcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastWrite;

        var observable = Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
                fileHandler => watcher.Changed += fileHandler,
                fileHandler => watcher.Changed -= fileHandler)
            .Throttle(TimeSpan.FromSeconds(0.1));

        IDisposable disposable = observable.Subscribe(evt => { CompileCommand(command, engine); });
        QuitEvent.WaitOne();
        disposable.Dispose();
        watcher.Dispose();
        return 0;
    }

    private static int CompileCommand(IEngine engine, string code)
    {
        int failedUnits = 0;
        _ = FrontCompiler.CompileUnitFile(code).Match(
            u =>
            {
                Console.WriteLine("Unit parsed");
                var files = engine.CompileUnit(u);
                foreach (SourceFile file in files)
                {
                    using var fileStream = File.CreateText($"{_conf.OutDir}/{file.FileName}.{file.Extension}");
                    fileStream.WriteLine(file.SourceCode);
                }

                return Unit.Default;
            },
            e =>
            {
                failedUnits++;
                Console.WriteLine(e.Message);
                return Unit.Default;
            }
        );

        if (failedUnits <= 0) return 0;
        Console.WriteLine("Total failed units {0}", failedUnits);
        return -1;
    }

    private static int CompileCommand(Compile command, IEngine engine)
    {
        int failedUnits = 0;
        _ = FrontCompiler.CompileUnitFile(command.Source).Match(
            u =>
            {
                Console.WriteLine($"Unit parsed: {command.Source}");
                var files = engine.CompileUnit(u);
                foreach (SourceFile file in files)
                {
                    using var fileStream = File.CreateText($"{_conf.OutDir}/{file.FileName}.{file.Extension}");
                    fileStream.WriteLine(file.SourceCode);
                }

                return Unit.Default;
            },
            e =>
            {
                failedUnits++;
                Console.WriteLine(e.Message);
                return Unit.Default;
            }
        );

        if (failedUnits <= 0) return 0;
        Console.WriteLine("Total failed units {0}", failedUnits);
        return -1;
    }
}