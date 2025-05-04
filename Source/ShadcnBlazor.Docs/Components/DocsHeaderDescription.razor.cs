using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Linq;

namespace ShadcnBlazor.Docs;
public partial class DocsHeaderDescription
{
    [Inject]
    IJSRuntime JSRuntime { get; set; } = default!;

    IJSObjectReference _jsModule;

    const string JSFile = "./_content/ShadcnBlazor.Docs/Components/DocsHeaderDescription.razor.js";
    bool _isDisposed;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JSFile);
            if (_jsModule != null)
            {
                await _jsModule.InvokeVoidAsync("init", Ref);
            }
        }
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeCoreAsync().ConfigureAwait(false);
        GC.SuppressFinalize(this);
    }

    protected virtual async ValueTask DisposeCoreAsync()
    {
        if (_isDisposed)
        {
            return;
        }

        if (_jsModule != null)
        {
            try
            {
                await _jsModule.InvokeVoidAsync("dispose", Ref).ConfigureAwait(false);
                await _jsModule.DisposeAsync().ConfigureAwait(false);
            }
            catch (InvalidOperationException)
            {
                // This can be called too early when using prerendering
            }
            catch (Exception ex) when (ex is JSDisconnectedException ||
                                       ex is OperationCanceledException)
            {
                // The JSRuntime side may routinely be gone already if the reason we're disposing is that
                // the client disconnected. This is not an error.
            }
        }
        _isDisposed = true;
    }
}
