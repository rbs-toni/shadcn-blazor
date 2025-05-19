using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ShadcnBlazor;
public partial class Tooltip : IAsyncDisposable
{
    const string JS_FILE = "./_content/ShadcnBlazor/Components/Tooltip/Tooltip.razor.js";
    readonly InternalTooltipContext _context;
    readonly SafeTimer _safeTimer = new();
    bool _hasPointerMoveOpened;
    bool _isOpenDelayed = true;
    bool _isPointerDown;
    bool _isPointerInTransit;
    IJSObjectReference? _jsModule;
    DotNetObjectReference<Tooltip>? _tooltip;
    bool _wasOpenDelayed;

    public Tooltip()
    {
        _context = new(this);
    }

    public string ContentId => _context.ContentId;
    public string DataState => IsShow
        ? (_wasOpenDelayed ? "delayed-open" : "instant-open")
        : "closed";
    [Inject]
    IJSRuntime JSRuntime { get; set; } = default!;

    #region Lifecycle

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JS_FILE);
        _tooltip ??= DotNetObjectReference.Create(this);
        await _jsModule.InvokeVoidAsync("init", AnchorId, _tooltip);
    }

    public async ValueTask DisposeAsync()
    {
        _safeTimer.Dispose();

        if (_jsModule is not null)
        {
            await _jsModule.InvokeVoidAsync("dispose", AnchorId);
            await _jsModule.DisposeAsync();
        }
    }

    #endregion

    #region JSInvokable

    [JSInvokable]
    public void HandleClick()
    {
        if (IsShow && !DisableClosingTrigger)
        {
            CloseTooltip();
            OnClose.InvokeAsync();
        }

        OnOpen.InvokeAsync();
    }

    [JSInvokable]
    public void HandleFocus()
    {
        if (_isPointerDown || IgnoreNonKeyboardFocus) return;

        OnOpen.InvokeAsync();
    }

    [JSInvokable]
    public void HandlePointerDown()
    {
        if (IsShow)
            OnClose.InvokeAsync();

        _isPointerDown = true;
    }

    [JSInvokable]
    public void HandlePointerLeave()
    {
        OnTriggerLeave();
        _hasPointerMoveOpened = false;
    }

    [JSInvokable]
    public void HandlePointerMove()
    {
        if (_hasPointerMoveOpened || _isPointerInTransit) return;

        if (_isOpenDelayed)
            OpenTooltip(delayed: true);
        else
            OpenTooltip();

        _hasPointerMoveOpened = true;
    }

    [JSInvokable]
    public void HandlePointerUp() => _isPointerDown = false;

    #endregion

    #region Tooltip Logic

    void OpenTooltip(bool delayed = false)
    {
        SetTimer(() =>
        {
            _wasOpenDelayed = delayed;

            if (!IsDisplay)
            {
                IsDisplay = true;
                StateHasChanged();
                return;
            }

            if (!IsShow)
                IsShow = true;
        });
    }

    void CloseTooltip()
    {
        SetTimer(() =>
        {
            _wasOpenDelayed = false;

            if (IsDisplay)
            {
                IsDisplay = false;
                StateHasChanged();
            }
        });
    }

    void OnRendered()
    {
        IsShow = true;
        StateHasChanged();
    }

    void OnTriggerEnter()
    {
        if (_isOpenDelayed)
            OpenTooltip(delayed: true);
        else
            OpenTooltip();
    }

    void OnTriggerLeave()
    {
        if (DisableHoverableContent)
        {
            CloseTooltip();
        }
        else
        {
            _wasOpenDelayed = false;
            if (IsDisplay)
            {
                IsDisplay = false;
                StateHasChanged();
            }
        }
    }

    void HandleBlur() => OnClose.InvokeAsync();

    #endregion

    #region Timer + Logging
    void SetTimer(Action callback) => _safeTimer.SetTimeout(DelayDuration, callback);
    void Log(string message, bool isIndented = false)
    {
        var prefix = isIndented ? new string(' ', "[Tooltip]: ".Length) : "[Tooltip]: ";
        Console.WriteLine($"{prefix}{message}");
    }
    #endregion
}
