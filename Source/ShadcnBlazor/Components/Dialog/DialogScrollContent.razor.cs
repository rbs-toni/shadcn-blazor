using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Linq;

namespace ShadcnBlazor;
public partial class DialogScrollContent : IAsyncDisposable
{
    const string JS_FILE = "./_content/ShadcnBlazor/Components/Dialog/DialogScrollContent.razor.js";
    IJSObjectReference? _jsModule;

    [Inject]
    IJSRuntime JSRuntime { get; set; } = default!;

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_jsModule != null)
            {
                await _jsModule.DisposeAsync();
            }
        }
        catch (Exception)
        {
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JS_FILE);
            if (_jsModule != null)
            {
                await _jsModule.InvokeVoidAsync("init", Ref);
            }
        }
    }
}
