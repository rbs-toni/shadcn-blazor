using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace ShadcnBlazor;
public class ScrollAreaScrollbarVisibleContext
{
    readonly ScrollAreaScrollbarVisible _scrollbarVisible;
    ScrollSize _sizes = new ScrollSize()
    {
        Content = 0,
        ViewPort = 0,
        Scrollbar = new Scrollbar
        {
            Size = 0,
            PaddingStart = 0,
            PaddingEnd = 0
        }
    };
    ElementReference _thumbRef;

    public ScrollAreaScrollbarVisibleContext(ScrollAreaScrollbarVisible scrollbarVisible)
    {
        _scrollbarVisible = scrollbarVisible;
    }

    public event Func<double, Task>? OnDragScroll;
    public event Action? OnSizeChange;
    public event Action? OnThumbChange;

    public bool HasThumb => _scrollbarVisible.HasThumb;
    public ScrollSize Sizes => _sizes;
    public ElementReference ThumbRef => _thumbRef;

    public void DragScroll(double delta)
    {
        OnDragScroll?.Invoke(delta);
    }
    public void HandleSizeChange(ScrollSize size)
    {
        _sizes = size;
        OnSizeChange?.Invoke();
    }
    public void HandleThumbDown(MouseEventArgs eventArgs, (double X, double Y) payload)
    {
        _scrollbarVisible.HandleThumbDown(eventArgs, payload);
    }
    public void HandleThumbUp(MouseEventArgs eventArgs)
    {
        _scrollbarVisible.HandleThumbUp(eventArgs);
    }
    public async Task HandleWheelScroll(WheelEventArgs args, double payload)
    {
        await _scrollbarVisible.HandleWheelScroll(args, payload);
    }
    public async Task OnThumbPositionChange()
    {
        await _scrollbarVisible.OnThumbPositionChange();
    }
    public void SetThumbRef(ElementReference thumbRef)
    {
        _thumbRef = thumbRef;
        OnThumbChange?.Invoke();
    }
}
