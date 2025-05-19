using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Linq;

namespace ShadcnBlazor;
public partial class SliderImpl<TValue> : IAsyncDisposable where TValue : System.Numerics.INumber<TValue>
{
    const string JS_FILE = "./_content/ShadcnBlazor/Components/Slider/SliderImpl.razor.js";
    IJSObjectReference? _disposeInstance;
    DotNetObjectReference<SliderImpl<TValue>>? _dotNet;
    IJSObjectReference? _jsModule;

    [Inject]
    IJSRuntime JSRuntime { get; set; } = default!;

    public async ValueTask DisposeAsync()
    {
        _dotNet?.Dispose();
        if (_disposeInstance != null)
        {
            await _disposeInstance.InvokeVoidAsync("dispose");
            await _disposeInstance.DisposeAsync();
        }
    }
    [JSInvokable]
    public void OnKeyDownHandler(KeyboardEventArgs args)
    {
        if (args.Key == "Home")
        {
            OnHomeKeyDown.InvokeAsync(args);
        }
        else if (args.Key == "End")
        {
            OnEndKeyDown.InvokeAsync(args);
        }
        else
        {
            OnStepKeyDown.InvokeAsync(args);
        }
    }
    [JSInvokable]
    public async Task OnPointerDownHandler(string id, PointerEventArgs args)
    {
        if (RootContext?.ThumbElementId == id)
        {
            await FocusAsync(id);
        }
        else
        {
            await OnSlideStart.InvokeAsync(args);
        }
    }
    async Task FocusAsync(string id)
    {
        if (_jsModule != null)
        {
            await _jsModule.InvokeVoidAsync("focus", id);
        }
    }
    [JSInvokable]
    public void OnPointerMoveHandler(PointerEventArgs args)
    {
        OnSlideMove.InvokeAsync(args);
    }
    [JSInvokable]
    public void OnPointerUpHandler(PointerEventArgs args)
    {
        OnSlideEnd.InvokeAsync(args);
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JS_FILE);
            if (_jsModule != null)
            {
                _dotNet ??= DotNetObjectReference.Create(this);
                _disposeInstance = await _jsModule.InvokeAsync<IJSObjectReference>("init", Ref, _dotNet);
            }
        }
    }
}
