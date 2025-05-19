using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Linq;
using static ShadcnBlazor.ScrollAreaUtils;

namespace ShadcnBlazor;
public partial class ScrollAreaScrollbarScroll : ShadcnJSComponentBase
{
    readonly StateMachine<ScrollState, ScrollEvent> _stateMachine;
    DotNetObjectReference<ScrollAreaScrollbarScroll>? _dotnet;
    SafeTimer _safeTimer;

    public ScrollAreaScrollbarScroll() : base(JSModulePathBuilder.GetComponentPath("ScrollArea", "ScrollAreaScrollbarScroll"))
    {
        _safeTimer = new SafeTimer();
        var transitions = new Dictionary<ScrollState, Dictionary<ScrollEvent, ScrollState>>
        {
            [ScrollState.Hidden] = new()
            {
                [ScrollEvent.Scroll] = ScrollState.Scrolling
            },
            [ScrollState.Scrolling] = new()
            {
                [ScrollEvent.ScrollEnd] = ScrollState.Idle,
                [ScrollEvent.PointerEnter] = ScrollState.Interacting
            },
            [ScrollState.Interacting] = new()
            {
                [ScrollEvent.Scroll] = ScrollState.Interacting,
                [ScrollEvent.PointerLeave] = ScrollState.Idle
            },
            [ScrollState.Idle] = new()
            {
                [ScrollEvent.Hide] = ScrollState.Hidden,
                [ScrollEvent.Scroll] = ScrollState.Scrolling,
                [ScrollEvent.PointerEnter] = ScrollState.Interacting
            }
        };
        _stateMachine = new StateMachine<ScrollState, ScrollEvent>(ScrollState.Hidden, transitions);
        _stateMachine.StateChanged += HandleScrollStateChange;
    }

    bool IsHorizontal => ScrollbarContext?.IsHorizontal ?? false;
    ElementReference? ViewPort => RootContext?.ViewPort;

    [JSInvokable]
    public void HandleScroll()
    {
        _stateMachine?.Dispatch(ScrollEvent.Scroll);
    }
    protected override async ValueTask OnAfterImportAsync()
    {
        await InitAsync();
    }
    protected override async ValueTask OnDisposingAsync()
    {
        _dotnet?.Dispose();
        _safeTimer?.Dispose();
        _stateMachine.StateChanged -= HandleScrollStateChange;
        await InvokeVoidAsync("dispose");
    }
    void HandleScrollStateChange(ScrollState state)
    {
        if (state == ScrollState.Idle)
        {
            _safeTimer.SetTimeout(RootContext?.ScrollHideDelay ?? 0, () => _stateMachine.Dispatch(ScrollEvent.Hide));
        }
    }
    async Task InitAsync()
    {
        _dotnet?.Dispose();
        await InvokeVoidAsync("init", ViewPort, IsHorizontal, _dotnet);
    }
}
