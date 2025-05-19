using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Linq;

namespace ShadcnBlazor;
public partial class ScrollAreaScrollbarHover : ShadcnJSComponentBase, IDisposable
{
    readonly SafeTimer _safeTimer;
    DotNetObjectReference<ScrollAreaScrollbarHover>? _dotNet;
    bool _refChanged;

    public ScrollAreaScrollbarHover() : base(JSModulePathBuilder.GetComponentPath("ScrollArea", "ScrollAreaScrollbarHover"))
    {
        _safeTimer = new SafeTimer();
    }

    ElementReference? ScrollArea => RootContext?.ScrollArea;
    int ScrollHideDelay => RootContext?.ScrollHideDelay ?? 0;

    public void Dispose()
    {
        _safeTimer?.Dispose();
    }
    [JSInvokable]
    public void OnPointerEnter()
    {
        HandlePointerEnter();
    }
    [JSInvokable]
    public void OnPointerLeave()
    {
        HandlePointerLeave();
    }
    protected override async ValueTask OnAfterImportAsync()
    {
        await InitAsync();
    }
    protected override async ValueTask OnDisposingAsync()
    {
        _safeTimer?.Dispose();
        _dotNet?.Dispose();
        await InvokeVoidAsync("dispose");
    }
    protected override void OnInitialized()
    {
        if (RootContext != null)
        {
            RootContext.OnScrollAreaChanged += OnScrollAreaChanged;
        }
    }
    void HandlePointerEnter()
    {
        _safeTimer.SetTimeout(_timeout, () =>
        {
            _visible = true;
            StateHasChanged();
        });
    }
    void HandlePointerLeave()
    {
        _safeTimer.SetTimeout(ScrollHideDelay, () =>
        {
            _visible = false;
            StateHasChanged();
        });
    }
    async Task InitAsync()
    {
        if (ScrollArea != null)
        {
            if (_refChanged)
            {
                await InvokeVoidAsync("dispose");
                _dotNet ??= DotNetObjectReference.Create(this);
                await InvokeVoidAsync("init", ScrollArea, _dotNet);
            }
            else
            {
                _dotNet ??= DotNetObjectReference.Create(this);
                await InvokeVoidAsync("init", ScrollArea, _dotNet);
            }
        }
    }
    void OnScrollAreaChanged()
    {
        _refChanged = true;
        InvokeAsync(InitAsync);
    }
}
