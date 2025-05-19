using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShadcnBlazor;
public partial class ScrollAreaThumb : ShadcnJSComponentBase
{
    DotNetObjectReference<ScrollAreaThumb>? _dotNet;
    IJSObjectReference? _pointerDownInstance;

    public ScrollAreaThumb() : base(JSModulePathBuilder.GetComponentPath("ScrollArea", "ScrollAreaThumb"))
    {
    }

    ElementReference? ViewPort => RootContext?.ViewPort;

    [JSInvokable]
    public void HandlePointerDown(PointerEventArgs e, double x, double y)
    {
        if (ScrollbarVisibleContext != null)
        {
            ScrollbarVisibleContext.HandleThumbDown(e, (x, y));
        }
    }
    [JSInvokable]
    public async Task HandleThumbPositionChange()
    {
        if (ScrollbarVisibleContext is not null)
        {
            await ScrollbarVisibleContext.OnThumbPositionChange();
        }
    }
    protected override async ValueTask OnAfterImportAsync()
    {
        if (ScrollbarVisibleContext != null)
        {
            ScrollbarVisibleContext.SetThumbRef(Ref);
            _dotNet ??= DotNetObjectReference.Create(this);
            if (ViewPort != null)
            {
                await InvokeVoidAsync("init", ViewPort, _dotNet, nameof(HandleThumbPositionChange));
            }
            _pointerDownInstance = await InvokeAsync<IJSObjectReference>("addPointerDown", Ref, _dotNet, nameof(HandlePointerDown));
        }
    }
    protected override async ValueTask OnDisposingAsync()
    {
        _dotNet?.Dispose();
        await InvokeVoidAsync("dispose");
        if (_pointerDownInstance != null)
        {
            await _pointerDownInstance.InvokeVoidAsync("cleanup");
        }
    }
    void HandlePointerUp(PointerEventArgs e)
    {
        ScrollbarVisibleContext?.HandleThumbUp(e);
    }
}
