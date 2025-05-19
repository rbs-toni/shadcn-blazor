using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Linq;

namespace ShadcnBlazor;
public partial class ScrollAreaScrollbarAuto : ShadcnJSComponentBase
{
    readonly DotNetObjectReference<ScrollAreaScrollbarAuto> _dotnet;

    public ScrollAreaScrollbarAuto() : base(JSModulePathBuilder.GetComponentPath("ScrollArea", "ScrollAreaScrollbarAuto"))
    {
        Id = Identifier.NewId();
        _dotnet ??= DotNetObjectReference.Create(this);
    }

    ElementReference? Content => RootContext?.Content;
    bool IsHorizontal => ScrollbarContext?.IsHorizontal ?? false;
    ElementReference? Viewport => RootContext?.ViewPort;

    [JSInvokable]
    public void HandleResize(bool isOverflowX, bool isOverflowY)
    {
        Console.WriteLine($"isOverflowX: {isOverflowX}");
        Console.WriteLine($"isOverflowY: {isOverflowY}");
        if (IsHorizontal)
        {
            _visible = isOverflowX;
            StateHasChanged();
        }
        else
        {
            _visible = isOverflowY;
            StateHasChanged();
        }
    }

    protected override async ValueTask OnAfterImportAsync()
    {
        await InvokeVoidAsync("useResizeObserver", Viewport, Content, _dotnet, nameof(HandleResize));
    }

    protected override async ValueTask OnDisposingAsync()
    {
        await InvokeVoidAsync("dispose");
    }
}
