using MediatR;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server.Capabilities;
using Serilog.Core;

namespace CrossBind.Lang;

public class TextDocumentHandler : ITextDocumentSyncHandler
{
    private readonly Logger _logger;

    private readonly DocumentSelector _documentSelector = new(
        new DocumentFilter
        {
            Pattern = "**/*.hbt",
            Language = "haibt",
            Scheme = "file"
        }
    );

    public TextDocumentHandler(Logger logger)
    {
        _logger = logger;
        _logger.Verbose("yei");
    }

    TextDocumentChangeRegistrationOptions
        IRegistration<TextDocumentChangeRegistrationOptions, SynchronizationCapability>.GetRegistrationOptions(
            SynchronizationCapability capability,
            ClientCapabilities clientCapabilities)
    {
        return new TextDocumentChangeRegistrationOptions
        {
            DocumentSelector = _documentSelector,
            SyncKind = TextDocumentSyncKind.Incremental,
        };
    }

    TextDocumentOpenRegistrationOptions IRegistration<TextDocumentOpenRegistrationOptions, SynchronizationCapability>.
        GetRegistrationOptions(SynchronizationCapability capability,
            ClientCapabilities clientCapabilities)
    {
        return new TextDocumentOpenRegistrationOptions
        {
            DocumentSelector = _documentSelector,
        };
    }

    TextDocumentCloseRegistrationOptions IRegistration<TextDocumentCloseRegistrationOptions, SynchronizationCapability>.
        GetRegistrationOptions(SynchronizationCapability capability,
            ClientCapabilities clientCapabilities)
    {
        return new TextDocumentCloseRegistrationOptions
        {
            DocumentSelector = _documentSelector,
        };
    }

    TextDocumentSaveRegistrationOptions IRegistration<TextDocumentSaveRegistrationOptions, SynchronizationCapability>.
        GetRegistrationOptions(SynchronizationCapability capability,
            ClientCapabilities clientCapabilities)
    {
        _logger.Information("Registering text saved");
        return new TextDocumentSaveRegistrationOptions
        {
            DocumentSelector = _documentSelector,
            IncludeText = true,
        };
    }

    public TextDocumentAttributes GetTextDocumentAttributes(DocumentUri uri)
    {
        return new TextDocumentAttributes(uri, "hbt");
    }

    public Task<Unit> Handle(DidSaveTextDocumentParams request, CancellationToken cancellationToken)
    {
        return Unit.Task;
    }

    public Task<Unit> Handle(DidOpenTextDocumentParams request, CancellationToken cancellationToken)
    {
        return Unit.Task;
    }

    public Task<Unit> Handle(DidChangeTextDocumentParams request, CancellationToken cancellationToken)
    {
        foreach (var change in request.ContentChanges)
        {
            Console.WriteLine("Text Change: {0}", change.Text);
        }

        return Unit.Task;
    }

    public Task<Unit> Handle(DidCloseTextDocumentParams request, CancellationToken cancellationToken)
    {
        _logger.Information("Registering close document");
        return Unit.Task;
    }
}