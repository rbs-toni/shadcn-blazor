using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Linq;

namespace ShadcnBlazor;
public partial class ToastGroup : IAsyncDisposable
{
    DotNetObjectReference<ToastGroup>? _dotNetObject;
    IJSObjectReference? _jsModule;
    IJSObjectReference? _toastGroup;

    [Inject]
    IJSRuntime JSRuntime { get; set; } = default!;

    public async ValueTask DisposeAsync()
    {
        _dotNetObject?.Dispose();
        if (_toastGroup != null)
        {
            await _toastGroup.InvokeVoidAsync("cleanUp");
            await _toastGroup.DisposeAsync();
        }
        if (_jsModule != null)
        {
            await _jsModule.DisposeAsync();
        }
    }
    [JSInvokable]
    public async Task InvokeOnExpanded(bool expanded)
    {
        await OnExpanded.InvokeAsync(expanded);
    }
    [JSInvokable]
    public async Task InvokeOnInteracting(bool interacting)
    {
        await OnInteracting.InvokeAsync(interacting);
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/ShadcnBlazor/Components/Toast/ToastGroup.razor.js");
            if (_jsModule != null)
            {
                _dotNetObject ??= DotNetObjectReference.Create(this);
                _toastGroup = await _jsModule.InvokeAsync<IJSObjectReference>("init", Ref, _dotNetObject);
            }
        }
    }
}
