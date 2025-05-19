using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ShadcnBlazor;
/// <summary>
/// Extends the OnKeyDown blazor event to provide a more fluent way to evaluate the key code.
/// The anchor must refer to the ID of an element (or sub-element) accepting the focus.
/// </summary>
public partial class ShadcnKeyCode : IAsyncDisposable
{
    /// <summary>
    /// Prevent multiple KeyDown events.
    /// </summary>
    public static bool PreventMultipleKeyDown;
    const string JSFile = "./_content/ShadcnBlazor/Components/KeyCode/ShadcnKeyCode.razor.js";
    readonly KeyCode[] _Modifiers = [KeyCode.Shift, KeyCode.Alt, KeyCode.Ctrl, KeyCode.Meta];
    DotNetObjectReference<ShadcnKeyCode>? _dotNetHelper;
    string _javaScriptEventId = string.Empty;

    
    IJSObjectReference? _jsModule { get; set; }
    ElementReference Element { get; set; }
    [Inject]
    IJSRuntime JSRuntime { get; set; } = default!;

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_jsModule != null && !string.IsNullOrEmpty(_javaScriptEventId))
            {
                await _jsModule.InvokeVoidAsync("UnregisterKeyCode", _javaScriptEventId);
                await _jsModule.DisposeAsync();
            }
        }
        catch (Exception ex) when (ex is JSDisconnectedException ||
                                   ex is OperationCanceledException)
        {
            // The JSRuntime side may routinely be gone already if the reason we're disposing is that
            // the client disconnected. This is not an error.
        }
    }
    /// <summary>
    /// Internal method.
    /// </summary>
    /// <param name="keyCode"></param>
    /// <param name="value"></param>
    /// <param name="ctrlKey"></param>
    /// <param name="shiftKey"></param>
    /// <param name="altKey"></param>
    /// <param name="metaKey"></param>
    /// <param name="location"></param>
    /// <param name="targetId"></param>
    /// <param name="repeat"></param>
    /// <returns></returns>
    [JSInvokable]
    public async Task OnKeyDownRaisedAsync(int keyCode, string value, bool ctrlKey, bool shiftKey, bool altKey, bool metaKey, int location, string targetId, bool repeat)
    {
        if (OnKeyDown.HasDelegate)
        {
            await OnKeyDown.InvokeAsync(KeyCodeEventArgs.Instance("keydown", keyCode, value, ctrlKey, shiftKey, altKey, metaKey, location, targetId, repeat));
        }
    }
    /// <summary>
    /// Internal method.
    /// </summary>
    /// <param name="keyCode"></param>
    /// <param name="value"></param>
    /// <param name="ctrlKey"></param>
    /// <param name="shiftKey"></param>
    /// <param name="altKey"></param>
    /// <param name="metaKey"></param>
    /// <param name="location"></param>
    /// <param name="targetId"></param>
    /// <param name="repeat"></param>
    /// <returns></returns>
    [JSInvokable]
    public async Task OnKeyUpRaisedAsync(int keyCode, string value, bool ctrlKey, bool shiftKey, bool altKey, bool metaKey, int location, string targetId, bool repeat)
    {
        if (OnKeyUp.HasDelegate)
        {
            await OnKeyUp.InvokeAsync(KeyCodeEventArgs.Instance("keyup", keyCode, value, ctrlKey, shiftKey, altKey, metaKey, location, targetId, repeat));
        }
    }
    /// <summary />
    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            if (ChildContent is null && string.IsNullOrEmpty(AnchorId) && !GlobalDocument)
            {
                throw new ArgumentNullException(AnchorId, $"The {nameof(AnchorId)} parameter must be set to the ID of an element. Or the {nameof(ChildContent)} must be set to apply the KeyCode engine to this content.");
            }

            _jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JSFile);
            _dotNetHelper = DotNetObjectReference.Create(this);

            var eventNames = string.Join(";",
            [
                OnKeyDown.HasDelegate ? "KeyDown" : string.Empty,
                OnKeyUp.HasDelegate ? "KeyUp" : string.Empty,
            ]);

            _javaScriptEventId = await _jsModule.InvokeAsync<string>("RegisterKeyCode", GlobalDocument, eventNames.Length > 1 ? eventNames : "KeyDown", AnchorId, ChildContent is null ? null : Element, Only, IgnoreModifier ? Ignore.Union(_Modifiers) : Ignore, StopPropagation, PreventDefault, PreventDefaultOnly, _dotNetHelper, PreventMultipleKeyDown, StopRepeat);
        }
    }
}

