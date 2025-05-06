using Microsoft.JSInterop;

namespace ShadcnBlazor;
public partial class Transition : ShadcnJSComponentBase
{
    DotNetObjectReference<Transition>? _dotNetHelper;
    bool _show;
    bool _shown;

    public Transition() : base("Transition/Transition")
    {
    }

    [JSInvokable]
    public async Task OnTransitionEndAsync(string eventName)
    {
        if (eventName == "AfterEnter")
        {
            _shown = true;
            await AfterEnter.InvokeAsync();
        }
        else if (eventName == "AfterLeave")
        {
            _shown = false;
            _show = false;
            StateHasChanged();
            await AfterLeave.InvokeAsync();
        }
    }
    protected async Task EnterAsync()
    { await InvokeVoidAsync("enter", Target, Name, _dotNetHelper, nameof(OnTransitionEndAsync)); }
    protected async Task LeaveAsync() { await InvokeVoidAsync("leave", Target); }
    protected override ValueTask OnAfterImportAsync()
    {
        _dotNetHelper ??= DotNetObjectReference.Create(this);

        return base.OnAfterImportAsync();
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (Show && _show)
        {
            await EnterAsync();
        }
    }
    protected override ValueTask OnDisposingAsync()
    {
        _dotNetHelper?.Dispose();
        return base.OnDisposingAsync();
    }
    protected override async Task OnParametersSetAsync()
    {
        if (Show)
        {
            _show = Show;
        }
        else if (JSAvailable && _shown)
        {
            await LeaveAsync();
        }
    }
}
