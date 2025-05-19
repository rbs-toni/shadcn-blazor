using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Linq;

namespace ShadcnBlazor;

public partial class ScrollAreaScrollbarImpl : ShadcnJSComponentBase
{
    DotNetObjectReference<ScrollAreaScrollbarImpl>? _dotnet;
    DomRect? _rect;

    public ScrollAreaScrollbarImpl() : base(
        JSModulePathBuilder.GetComponentPath("ScrollArea", "ScrollAreaScrollbarImpl"))
    { Id = Identifier.NewId(); }

    ElementReference? Scrollbar => RootContext?.ViewPort;

    [JSInvokable]
    public async Task HandleSizeChangeAsync()
    {
        if (Scrollbar == null)
        {
            return;
        }

        var viewportInfo = await GetElementInfoAsync(RootContext?.ViewPort);
        var scrollbarInfo = await GetElementInfoAsync(Scrollbar);

        var paddingStart = await GetPropertyValueAsync(Scrollbar, "padding-left");
        var paddingEnd = await GetPropertyValueAsync(Scrollbar, "padding-right");

        if (viewportInfo == null || scrollbarInfo == null)
        {
            return;
        }

        var size = new ScrollSize
        {
            Content = IsHorizontal ? viewportInfo.ScrollWidth : viewportInfo.ScrollHeight,
            ViewPort = IsHorizontal ? viewportInfo.OffsetWidth : viewportInfo.OffsetHeight,
            Scrollbar =
                new Scrollbar
                {
                    Size = IsHorizontal ? scrollbarInfo.ClientWidth : scrollbarInfo.ClientHeight,
                    PaddingStart = paddingStart,
                    PaddingEnd = paddingEnd
                }
        };
        Console.WriteLine($"HandleSizeChangeAsync: {size}");

        ScrollbarVisibleContext?.HandleSizeChange(size);
    }

    [JSInvokable]
    public void HandleWheel(WheelEventArgs args)
    {
        var maxScrollPos = ScrollbarVisibleContext?.Sizes?.Content - ScrollbarVisibleContext?.Sizes?.ViewPort;
        ScrollbarVisibleContext?.HandleWheelScroll(args, maxScrollPos ?? 0);
    }

    IJSObjectReference? _resizeObserver;

    ElementReference? ViewPort => RootContext?.ViewPort;

    ElementReference? Content => RootContext?.Content;

    protected override async ValueTask OnAfterImportAsync()
    {
        _dotnet ??= DotNetObjectReference.Create(this);
        await InvokeVoidAsync("addWheelListener", Scrollbar, _dotnet, nameof(HandleWheel));
        _resizeObserver = await InvokeAsync<IJSObjectReference>(
            "addResizeObserver",
            ViewPort,
            Scrollbar,
            Content,
            IsHorizontal,
            _dotnet,
            nameof(HandleSizeChangeAsync));
    }

    protected override async ValueTask OnDisposingAsync()
    {
        await InvokeVoidAsync("removeWheelListener");
        if (_resizeObserver != null)
        {
            await _resizeObserver.InvokeVoidAsync("dispose");
            await  _resizeObserver.DisposeAsync();
        }
    }

    async Task<DomRect> GetBoundingClientRectAsync(ElementReference element)
    { return await InvokeAsync<DomRect>("getBoundingClientRect", element); }
    async Task<ElementInfo> GetElementInfoAsync(ElementReference? element)
    { return await InvokeAsync<ElementInfo>("getElementInfo", element); }
    async Task<int> GetPropertyValueAsync(ElementReference? element, string prop)
    { return await InvokeAsync<int>("getComputedStyle", element, prop); }

    void HandleDragScroll(MouseEventArgs args)
    {
        if (_rect != null)
        {
            var x = args.ClientX - _rect.Left;
            var y = args.ClientY - _rect.Top;
            if (OnDragScroll.HasDelegate)
            {
                OnDragScroll.InvokeAsync((x, y));
            }
        }
    }

    async Task HandlePointerDown(ExtendedPointerEventArgs args)
    {
        var mainPointer = 0;
        if (args.Button == mainPointer)
        {
            var element = args.Target;
            await SetPointerCaptureAsync(element, args.PointerId);
            var rect = await GetBoundingClientRectAsync(Ref);
            if (rect != null)
            {
                _rect = rect;
            }
            if (Scrollbar != null)
            {
                await SetElementStyleAsync(Scrollbar, "scroll-behavior", "auto");
            }
            HandleDragScroll(args);
        }
    }

    void HandlePointerMove(ExtendedPointerEventArgs args) { HandleDragScroll(args); }

    async Task HandlePointerUp(ExtendedPointerEventArgs args)
    {
        var element = args.Target;
        await ReleasePointerCaptureasyn(element, args.PointerId);
        await SetElementStyleAsync(Scrollbar, "scroll-behavior", string.Empty);
        _rect = null;
    }

    async Task ReleasePointerCaptureasyn(string? id, long pointerId)
    { await InvokeVoidAsync("releasePointerCapture", id, pointerId); }
    async Task SetElementStyleAsync(ElementReference? element, string property, string value)
    { await InvokeVoidAsync("setElementStyle", element, property, value); }
    async Task SetPointerCaptureAsync(string? id, long pointerId)
    { await InvokeVoidAsync("setPointerCapture", id, pointerId); }

    record ElementInfo
    {
        public double ClientHeight { get; set; }

        public double ClientWidth { get; set; }

        public double OffsetHeight { get; set; }

        public double OffsetWidth { get; set; }

        public double ScrollHeight { get; set; }

        public double ScrollWidth { get; set; }
    }
}
