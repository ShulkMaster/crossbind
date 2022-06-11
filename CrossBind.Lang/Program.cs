using OmniSharp.Extensions.LanguageServer.Server;
using Serilog;
using Serilog.Extensions.Logging;

namespace CrossBind.Lang;

public static class Program
{
    public static async Task Main()
    {
        var serilog = new LoggerConfiguration()
            .Enrich
            .FromLogContext()
            .WriteTo.File("NEPELOG.txt")
            .CreateLogger();
        Log.Logger = serilog;
        serilog.Information("A la verga");

        var server = LanguageServer.Create(options =>
        {
            options.WithOutput(Console.OpenStandardOutput());
            options.WithInput(Console.OpenStandardInput());
            options.LoggerFactory = new SerilogLoggerFactory(serilog);
            options.AddHandler(new TextDocumentHandler(serilog));
            options.AddHandler(new CompletionHandler(serilog));
        });
        await server.Initialize(default);
        await server.WaitForExit;
    }
}