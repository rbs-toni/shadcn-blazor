using Microsoft.AspNetCore.Components;
using System;
using System.Linq;

namespace ShadcnBlazor;
public class ScrollAreaRootContext
{
    readonly ScrollAreaRoot _scrollAreaRoot;
    ElementReference _content;
    double _cornerHeight;
    double _cornerWidth;
    ElementReference _scrollArea;
    ElementReference _scrollbarX;
    bool _scrollbarXEnabled;
    ElementReference _scrollbarY;
    bool _scrollbarYEnabled;
    ElementReference _viewPort;

    public ScrollAreaRootContext(ScrollAreaRoot scrollAreaRoot) { _scrollAreaRoot = scrollAreaRoot; }

    public event Action? OnContentChange;
    public event Action? OnScrollAreaChanged;
    public event Action? OnScrollbarXChange;
    public event Action? OnScrollbarXEnabledChange;
    public event Action? OnScrollbarYChange;
    public event Action? OnScrollbarYEnabledChange;
    public event Action? OnViewportChange;

    public ElementReference Content => _content;
    public double CornerHeight => _cornerHeight;
    public double CornerWidth => _cornerWidth;
    public Direction Direction => _scrollAreaRoot.Direction;
    public Action<double>? OnCornerHeightChange { get; set; }
    public Action<double>? OnCornerWidthChange { get; set; }
    public ElementReference ScrollArea => _scrollArea;
    public ElementReference ScrollbarX => _scrollbarX;
    public bool ScrollbarXEnabled => _scrollbarXEnabled;
    public ElementReference ScrollbarY => _scrollbarY;
    public bool ScrollbarYEnabled => _scrollbarYEnabled;
    public int ScrollHideDelay => _scrollAreaRoot.ScrollHideDelay;
    public ScrollType ScrollType => _scrollAreaRoot.Type;
    public ElementReference ViewPort => _viewPort;

    public void SetContent(ElementReference element)
    {
        _content = element;
        OnContentChange?.Invoke();
    }
    public void SetCornerHeight(double height)
    {
        _cornerHeight = height;
        OnCornerHeightChange?.Invoke(height);
    }
    public void SetCornerWidth(double width)
    {
        _cornerWidth = width;
        OnCornerWidthChange?.Invoke(width);
    }
    public void SetScrollArea(ElementReference scrollArea)
    {
        _scrollArea = scrollArea;
        OnScrollAreaChanged?.Invoke();
    }
    public void SetScrollbarX(ElementReference scrollbarX)
    {
        _scrollbarX = scrollbarX;
        OnScrollbarXChange?.Invoke();
    }
    public void SetScrollbarXEnabled(bool enabled)
    {
        _scrollbarXEnabled = enabled;
        OnScrollbarXEnabledChange?.Invoke();
    }
    public void SetScrollbarY(ElementReference scrollbarY)
    {
        _scrollbarY = scrollbarY;
        OnScrollbarYChange?.Invoke();
    }
    public void SetScrollbarYEnabled(bool enabled)
    {
        _scrollbarYEnabled = enabled;
        OnScrollbarYEnabledChange?.Invoke();
    }
    public void SetViewport(ElementReference element)
    {
        _viewPort = element;
        OnViewportChange?.Invoke();
    }
}
