using CrossBind.Lang.Word;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using Serilog.Core;

namespace CrossBind.Lang;

public class CompletionHandler : ICompletionHandler
{
    private readonly Logger _logger;
    private int _count = 0;

    public CompletionHandler(Logger logger)
    {
        _logger = logger;
    }

    public async Task<CompletionList> Handle(CompletionParams request, CancellationToken cancellationToken)
    {
        _count++;
        var words = Keyword.All.Select(w => new CompletionItem
        {
            Label = w,
            Kind = CompletionItemKind.Keyword,
            InsertText = w,
        });
        words = words.Append(
            new CompletionItem
            {
                Label = $"Intento {_count}",
                Kind = CompletionItemKind.Constant,
                InsertText = "HoliFromCSharp",
            });
        await Task.Yield();
        return new CompletionList(words.ToArray());
    }

    public CompletionRegistrationOptions GetRegistrationOptions(CompletionCapability capability,
        ClientCapabilities clientCapabilities)
    {
        return new CompletionRegistrationOptions
        {
            DocumentSelector = new DocumentSelector(
                new DocumentFilter
                {
                    Pattern = "**/*.hbt",
                    Language = "haibt",
                    Scheme = "file"
                }
            )
        };
    }
}