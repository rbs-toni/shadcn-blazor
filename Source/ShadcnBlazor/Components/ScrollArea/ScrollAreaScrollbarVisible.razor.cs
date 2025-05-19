using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Linq;

namespace ShadcnBlazor;
public partial class ScrollAreaScrollbarVisible : ShadcnJSComponentBase
{
    readonly ScrollAreaScrollbarVisibleContext _context;
    double _pointerOffset;

    public ScrollAreaScrollbarVisible() : base(JSModulePathBuilder.GetComponentPath("ScrollArea", "ScrollAreaScrollbarVisible"))
    {
        Id = Identifier.NewId();
        _context = new(this);
        _context.OnDragScroll += OnDragScroll;
        _context.OnSizeChange += () => InvokeAsync(StateHasChanged);
    }

    internal bool HasThumb
    {
        get
        {
            var thumbRatio = ScrollAreaUtils.GetThumbRatio(Sizes.ViewPort, Sizes.Content);
            return thumbRatio > 0 && thumbRatio < 1;
        }
    }
    internal ScrollSize Sizes => _context.Sizes;
    Direction Direction => RootContext?.Direction ?? Direction.LTR;
    bool IsShowingScrollbarX => ScrollbarContext?.IsHorizontal ?? false;
    ElementReference ThumbRef => _context.ThumbRef;
    ElementReference? ViewPort => RootContext?.ViewPort;

    internal void HandleThumbDown(MouseEventArgs eventArgs, (double X, double Y) payload)
    {
        if (IsShowingScrollbarX)
        {
            _pointerOffset = payload.X;
        }
        else
        {
            _pointerOffset = payload.Y;
        }
    }
    internal void HandleThumbUp(MouseEventArgs eventArgs)
    {
        _pointerOffset = 0;
    }
    internal async Task HandleWheelScroll(WheelEventArgs args, double payload)
    {
        var viewPortInfo = await GetElementInfoAsync(ViewPort);
        if (IsShowingScrollbarX)
        {
            var scrollPos = viewPortInfo?.ScrollLeft ?? 0 + args.DeltaY;
            await SetElementInfoAsync(ViewPort, "scrollLeft", scrollPos);
        }
        else
        {
            var scrollPos = viewPortInfo?.ScrollTop ?? 0 + args.DeltaY;
            await SetElementInfoAsync(ViewPort, "scrollTop", scrollPos);
        }
    }
    internal async Task OnThumbPositionChange()
    {
        var viewportInfo = await GetElementInfoAsync(ViewPort);
        if (IsShowingScrollbarX)
        {
            var scrollPos = viewportInfo?.ScrollLeft ?? 0;
            var offset = ScrollAreaUtils.GetThumbOffsetFromScroll(scrollPos, Sizes, Direction);
            await SetElementInfoAsync(ThumbRef, "style.transform", $"translate3d({offset}px, 0, 0)");
        }
        else
        {
            var scrollPos = viewportInfo?.ScrollTop ?? 0;
            var offset = ScrollAreaUtils.GetThumbOffsetFromScroll(scrollPos, Sizes, Direction);
            await SetElementInfoAsync(ThumbRef, "style.transform", $"translate3d(0, {offset}px, 0)");
        }
    }
    protected override ValueTask OnDisposingAsync()
    {
        _context.OnDragScroll -= OnDragScroll;
        return base.OnDisposingAsync();
    }
    async Task<ElementInfo?> GetElementInfoAsync(ElementReference? element)
    {
        if (element != null)
        {
            return await InvokeAsync<ElementInfo>("getElementInfo", element);
        }

        return default;
    }
    double GetScrollPosition(double pointerPos, Direction direction)
    {
        return GetScrollPositionFromPointer(pointerPos, _pointerOffset, Sizes, direction);
    }
    double GetScrollPositionFromPointer(double pointerPos, double pointerOffset, ScrollSize sizes, Direction direction)
    {
        var thumbSizePx = ScrollAreaUtils.GetThumbSize(sizes);
        var thumbCenter = thumbSizePx / 2;
        var offset = pointerOffset != 0 ? pointerOffset : thumbCenter;
        var thumbOffsetFromEnd = thumbSizePx - offset;
        var minPointerPos = sizes.Scrollbar?.PaddingStart + offset;
        var maxPointerPos = sizes.Scrollbar?.Size - sizes.Scrollbar?.PaddingEnd - thumbOffsetFromEnd;
        var maxScrollPos = sizes.Content - sizes.ViewPort;
        var scrollRange = direction == Direction.LTR ? (0d, maxScrollPos) : (maxScrollPos * -1, 0d);
        var interpolate = ScrollAreaUtils.LinearScale((minPointerPos ?? 0, maxPointerPos ?? 0), scrollRange);

        return interpolate(pointerPos);
    }
    async Task OnDragScroll(double payload)
    {
        if (IsShowingScrollbarX)
        {
            await SetElementInfoAsync(ViewPort, "scrollLeft", GetScrollPosition(payload, Direction));
        }
        else
        {
            await SetElementInfoAsync(ViewPort, "scrollTop", GetScrollPosition(payload, Direction));
        }
    }
    async Task SetElementInfoAsync(ElementReference? element, string prop, object value)
    {
        if (element != null)
        {
            await InvokeVoidAsync("setElementInfo", element, prop, value);
        }
    }

    record ElementInfo
    {
        public double ClientHeight { get; set; }
        public double ClientWidth { get; set; }
        public double OffsetHeight { get; set; }
        public double OffsetWidth { get; set; }
        public double ScrollHeight { get; set; }
        public double ScrollLeft { get; set; }
        public double ScrollTop { get; set; }
        public double ScrollWidth { get; set; }
    }
}
