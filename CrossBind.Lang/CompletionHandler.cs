using Newtonsoft.Json;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using Serilog.Core;

namespace CrossBind.Lang;

public class CompletionHandler: ICompletionHandler
{

    private readonly Logger _logger;
    
    public CompletionHandler(Logger logger)
    {
        _logger = logger;
    }
    
    public async Task<CompletionList> Handle(CompletionParams request, CancellationToken cancellationToken)
    {
        _logger.Information("{Data}", JsonConvert.SerializeObject(request));
        await Task.Yield();
        return new CompletionList(new []
        {
            new CompletionItem
            {
                Label = "CSharp",
                Kind = CompletionItemKind.Keyword,
                InsertText = "HoliFromCSharp",
                FilterText = "HolaFromC",
            },
            new CompletionItem
            {
                Label = "C#",
                Kind = CompletionItemKind.Field,
                InsertText = "HoliFromC#",
                FilterText = "HolaFromC",
            }
        });
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