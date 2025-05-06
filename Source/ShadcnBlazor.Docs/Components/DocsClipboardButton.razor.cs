using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Linq;

namespace ShadcnBlazor.Docs;
public partial class DocsClipboardButton : IAsyncDisposable
{
    [Inject]
    IJSRuntime JSRuntime { get; set; } = default!;

    IJSObjectReference? _jsModule;

    const string JSFile = "./_content/ShadcnBlazor.Docs/Components/DocsClipboardButton.razor.js";

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JSFile);
            if (_jsModule != null)
            {
                await _jsModule.InvokeVoidAsync("init", Ref, CodeTarget);
            }
        }
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_jsModule != null)
            {
                await _jsModule.InvokeVoidAsync("dispose", Ref);
                await _jsModule.DisposeAsync();
            }
        }
        catch
        {
        }
        finally
        {
            GC.SuppressFinalize(this);
        }
    }
}
