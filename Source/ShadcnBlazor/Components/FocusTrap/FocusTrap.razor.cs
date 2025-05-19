using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace ShadcnBlazor;
/// <summary>
/// A component which prevents the keyboard focus from cycling out of its child content.
/// </summary>
/// <remarks>
/// Typically used within dialogs and other overlays.
/// </remarks>
public partial class FocusTrap : IDisposable
{
    protected ElementReference _fallback;
    protected ElementReference _firstBumper;
    protected ElementReference _lastBumper;
    protected ElementReference _root;
    private bool _disabled;
    private bool _initialized;
    private bool _shiftDown;
    private bool _shouldRender = true;

    /// <summary>
    /// The element which receives focus when this focus trap is created or enabled.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="DefaultFocus.FirstChild"/>.
    /// </remarks>
    [Parameter]
    public DefaultFocus DefaultFocus { get; set; } = DefaultFocus.FirstChild;
    /// <summary>
    /// Prevents the user from interacting with this focus trap.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>false</c>.
    /// </remarks>
    [Parameter]
    public bool Disabled
    {
        get => _disabled;
        set
        {
            if (_disabled != value)
            {
                _disabled = value;
                _initialized = false;
            }
        }
    }
    protected string? ClassValue =>
        new CssBuilder()
            .AddClass("outline-none")
            .AddClass(Class)
            .Build();
    private string TrapTabIndex => Disabled ? "-1" : "0";

    /// <summary>
    /// Releases resources used by this focus trap.
    /// </summary>
    public void Dispose()
    {
        if (!_disabled)
        {
            RestoreFocusAsync();
        }
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            await SaveFocusAsync();
        }

        if (!_initialized)
        {
            await InitializeFocusAsync();
        }
    }
    protected override bool ShouldRender()
    {
        if (_shouldRender)
        {
            return true;
        }

        _shouldRender = true; // auto-reset _shouldRender to true

        return false;
    }
    private Task FocusFallbackAsync()
    {
        return _fallback.FocusAsync().AsTask();
    }
    private Task FocusFirstAsync()
    {
        return _root.FocusAsync().AsTask();
    }
    private Task FocusLastAsync()
    {
        return _root.FocusAsync().AsTask();
    }
    private void HandleKeyEvent(KeyboardEventArgs args)
    {
        _shouldRender = false;
        if (args.Key == "Tab")
        {
            _shiftDown = args.ShiftKey;
        }
    }
    private Task InitializeFocusAsync()
    {
        _initialized = true;

        if (!_disabled)
        {
            switch (DefaultFocus)
            {
                case DefaultFocus.Element: return FocusFallbackAsync();
                case DefaultFocus.FirstChild: return FocusFirstAsync();
                case DefaultFocus.LastChild: return FocusLastAsync();
            }
        }
        return Task.CompletedTask;
    }
    private Task OnBottomFocusAsync(FocusEventArgs args)
    {
        return FocusLastAsync();
    }
    private Task OnBumperFocusAsync(FocusEventArgs args)
    {
        return _shiftDown ? FocusLastAsync() : FocusFirstAsync();
    }
    private Task OnRootFocusAsync(FocusEventArgs args)
    {
        return FocusFallbackAsync();
    }
    private void OnRootKeyDown(KeyboardEventArgs args)
    {
        HandleKeyEvent(args);
    }
    private void OnRootKeyUp(KeyboardEventArgs args)
    {
        HandleKeyEvent(args);
    }
    private Task OnTopFocusAsync(FocusEventArgs args)
    {
        return FocusFirstAsync();
    }
    private Task RestoreFocusAsync()
    {
        return _root.FocusAsync().AsTask();
    }
    private Task SaveFocusAsync()
    {
        return _root.FocusAsync().AsTask();
    }
}
