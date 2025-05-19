using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShadcnBlazor;
public partial class ScrollAreaCornerImpl : ShadcnJSComponentBase
{
    readonly DotNetObjectReference<ScrollAreaCornerImpl> _dotnet;

    public ScrollAreaCornerImpl() : base(JSModulePathBuilder.GetComponentPath("ScrollArea", "ScrollAreaCornerImpl"))
    { _dotnet = DotNetObjectReference.Create(this); }

    ElementReference? ScrollbarX => RootContext?.ScrollbarX;
    ElementReference? ScrollbarY => RootContext?.ScrollbarY;

    [JSInvokable]
    public void SetCornerHeight(double height)
    {
        _height = height;
        StateHasChanged();
    }
    [JSInvokable]
    public void SetCornerWidth(double width)
    {
        _width = width;
        StateHasChanged();
    }
    protected override async ValueTask OnDisposingAsync()
    {
        _dotnet?.Dispose();
        if (RootContext != null)
        {
            RootContext.OnScrollbarXChange -= ObserverScrollbarX;
            RootContext.OnScrollbarXChange -= ObserverScrollbarY;
        }
        await InvokeVoidAsync("dispose");
    }
    protected override void OnInitialized()
    {
        if (RootContext != null)
        {
            RootContext.OnScrollbarXChange += ObserverScrollbarX;
            RootContext.OnScrollbarXChange += ObserverScrollbarY;
        }
    }
    void ObserverScrollbarX()
    {
        if (ScrollbarX != null)
        {
            InvokeAsync(
                async () => await InvokeVoidAsync("observeScrollbarX", ScrollbarX, _dotnet, nameof(SetCornerHeight)));
        }
    }
    void ObserverScrollbarY()
    {
        if (ScrollbarY != null)
        {
            InvokeAsync(
                async () => await InvokeVoidAsync("observeScrollbarY", ScrollbarY, _dotnet, nameof(SetCornerWidth)));
        }
    }
}
