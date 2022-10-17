using Antlr4.Runtime;
using CrossBind.Lang.Text;
using MediatR;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using OmniSharp.Extensions.LanguageServer.Protocol.Server.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Window;

namespace CrossBind.Lang.Handlers;

public class TextDocumentSyncHandler : ITextDocumentSyncHandler
{
    private readonly ILanguageServer _router;
    private readonly BufferManager _bufferManager;
    private readonly TextDocumentSyncKind _sync = TextDocumentSyncKind.Full;

    private readonly DocumentSelector _documentSelector = new(
        new DocumentFilter
        {
            Pattern = "**/*.hbt"
        }
    );

    public TextDocumentSyncHandler(ILanguageServer router, BufferManager bufferManager)
    {
        _router = router;
        _bufferManager = bufferManager;
    }

    public TextDocumentAttributes GetTextDocumentAttributes(DocumentUri uri)
    {
        return new TextDocumentAttributes(uri, "haibt");
    }

    TextDocumentOpenRegistrationOptions IRegistration<TextDocumentOpenRegistrationOptions, SynchronizationCapability>.
        GetRegistrationOptions(SynchronizationCapability a,
            ClientCapabilities b)
    {
        return new TextDocumentOpenRegistrationOptions
        {
            DocumentSelector = _documentSelector,
        };
    }

    TextDocumentChangeRegistrationOptions
        IRegistration<TextDocumentChangeRegistrationOptions, SynchronizationCapability>.GetRegistrationOptions(
            SynchronizationCapability a,
            ClientCapabilities b)
    {
        return new TextDocumentChangeRegistrationOptions
        {
            DocumentSelector = _documentSelector,
            SyncKind = _sync
        };
    }


    TextDocumentCloseRegistrationOptions IRegistration<TextDocumentCloseRegistrationOptions, SynchronizationCapability>.
        GetRegistrationOptions(SynchronizationCapability a,
            ClientCapabilities b)
    {
        return new TextDocumentCloseRegistrationOptions
        {
            DocumentSelector = _documentSelector,
        };
    }

    TextDocumentSaveRegistrationOptions IRegistration<TextDocumentSaveRegistrationOptions, SynchronizationCapability>.
        GetRegistrationOptions(SynchronizationCapability a,
            ClientCapabilities b)
    {
        return new TextDocumentSaveRegistrationOptions
        {
            DocumentSelector = _documentSelector,
        };
    }

    public Task<Unit> Handle(DidCloseTextDocumentParams request, CancellationToken cancellationToken)
    {
        return Unit.Task;
    }


    public Task<Unit> Handle(DidChangeTextDocumentParams request, CancellationToken cancellationToken)
    {
        var documentPath = request.TextDocument.Uri.ToString();
        var text = request.ContentChanges.FirstOrDefault()?.Text;

        _bufferManager.UpdateBuffer(documentPath, new AntlrInputStream(text));

        _router.Window.LogInfo($"Updated buffer for document: {documentPath}");

        return Unit.Task;
    }

    public Task<Unit> Handle(DidOpenTextDocumentParams request, CancellationToken cancellationToken)
    {
        var text = request.TextDocument.Text;
        var uri = request.TextDocument.Uri.ToString();
        _bufferManager.UpdateBuffer(uri, new AntlrInputStream(text));

        _router.Window.LogInfo($"Open buffer for document: {uri}");

        return Unit.Task;
    }

    public Task<Unit> Handle(DidSaveTextDocumentParams request, CancellationToken cancellationToken)
    {
        var text = request.Text;
        var uri = request.TextDocument.Uri.ToString();
        _bufferManager.UpdateBuffer(uri, new AntlrInputStream(text));

        _router.Window.LogInfo($"Saved buffer for document: {uri}");

        return Unit.Task;
    }
}