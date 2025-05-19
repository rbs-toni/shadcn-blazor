using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ShadcnBlazor;
public partial class Toast : IAsyncDisposable
{
    CountdownTimer? _countdownTimer;
    DotNetObjectReference<Toast>? _dotNetObject;
    IJSObjectReference? _jsModule;

    [Inject]
    IJSRuntime JSRuntime { get; set; } = default!;

    public async Task DeleteToastAsync()
    {
        _removed = true;
        _offsetBeforeRemove = Offset;
        var heights = Heights?.Where(x => x.ToastId != Instance?.Id).ToList();
        if (OnUpdateHeights.HasDelegate)
        {
            await OnUpdateHeights.InvokeAsync(heights);
        }
        await Task.Delay(TIME_BEFORE_UNMOUNT);
        if (OnRemoveToast.HasDelegate)
        {
            if (_jsModule != null)
            {
                try
                {
                    await _jsModule.InvokeVoidAsync("dispose");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
            if (Instance != null)
                await OnRemoveToast.InvokeAsync(Instance);
            else
                Console.WriteLine("Instance is null when trying to remove toast");
        }
    }
    public async ValueTask DisposeAsync()
    {
        _countdownTimer?.Dispose();
        _countdownTimer = null;
        _dotNetObject?.Dispose();
        if (_jsModule != null)
        {
            await _jsModule.DisposeAsync();
        }
        var newHeights = Heights?.Where(x => x.ToastId != Instance?.Id).ToList();
        if (OnUpdateHeights.HasDelegate)
        {
            await OnUpdateHeights.InvokeAsync(newHeights);
        }
    }
    [JSInvokable]
    public void OnDocumentIsHidden(bool hidden)
    {
        _isDocumentHidden = hidden;
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/ShadcnBlazor/Components/Toast/Toast.razor.js");
                if (_jsModule != null)
                {
                    _dotNetObject ??= DotNetObjectReference.Create(this);
                    await _jsModule.InvokeVoidAsync("init", _dotNetObject, nameof(OnDocumentIsHidden));
                    await UpdateFirstHeights();
                }
                else
                {
                    Console.WriteLine("_jsModule is null");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }
        if (_mounted)
        {
            if (Expanded || Interacting || (PauseWhenPageIsHidden == true && _isDocumentHidden))
            {
                if (!_paused)
                {
                    TryPauseCountdown();
                }
            }
            else
            {
                if (_paused)
                {
                    TryResumeCountdown();
                }
            }
        }
    }
    protected override async Task OnInitializedAsync()
    {
        _remainingTime =
            (Instance?.Duration is > 0 ? Instance.Duration :
            Duration is > 0 ? Duration :
            TOAST_LIFETIME).Value;
        var timeout = _remainingTime / 1_000;
        _countdownTimer = new CountdownTimer(timeout ?? TOAST_LIFETIME / 1000)
            .OnElapsed(() => InvokeAsync(DeleteToastAsync));

        await _countdownTimer.StartAsync();
    }
    async Task<bool> GetSelectionAsync()
    {
        if (_jsModule != null)
        {
            return await _jsModule.InvokeAsync<bool>("getWindowSelection");
        }
        return false;
    }
    async Task<double> GetSwipeAmountAsync()
    {
        if (_jsModule != null)
        {
            return await _jsModule.InvokeAsync<double>("getSwipeAmount", Ref);
        }
        return default;
    }
    async Task HandleCloseToast()
    {
        if (ToastDisabled || !Dismissible)
        {
            return;
        }
        await DeleteToastAsync();
        if (Instance?.OnDismiss?.HasDelegate == true)
        {
            await Instance.OnDismiss?.InvokeAsync(Instance);
        }
    }
    async Task SetPointerCaptureAsync(long pointerId)
    {
        if (_jsModule != null)
        {
            await _jsModule.InvokeVoidAsync("setElementPointerCapture", Ref, pointerId);
        }
    }
    void TryPauseCountdown()
    {
        _countdownTimer?.Pause();
        _paused = true;
    }
    void TryResumeCountdown()
    {
        _countdownTimer?.UnPause();
        _paused = false;
    }
    async Task UpdateFirstHeights()
    {
        if (Instance is null) return;

        _mounted = true;

        var dimensions = await Ref.GetBoundingClientRectAsync(ElementService);
        _initialHeight = dimensions.Height;


        var justNewHeight = new ToastHeight(Instance.Id, Instance.Position, _initialHeight);

        Heights = Heights is { Count: > 0 }
       ? [justNewHeight, .. Heights]
       : [justNewHeight];
        await OnUpdateHeights.InvokeAsync(Heights);
    }

    bool _paused;

    async Task ButtonActionClickHandler()
    {
        if (Instance?.Action?.OnClick == null)
        {
            return;
        }
        if (!Dismissible)
        {
            return;
        }
        await Instance.Action.OnClick.InvokeAsync();
        await DeleteToastAsync();
    }
    async Task ButtonCancelClickHandler()
    {

        if (Instance?.Cancel?.OnClick == null)
        {
            return;
        }
        if (!Dismissible)
        {
            return;
        }
        await Instance.Cancel.OnClick.InvokeAsync();
        await DeleteToastAsync();
    }
}
