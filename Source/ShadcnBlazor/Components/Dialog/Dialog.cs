using Microsoft.AspNetCore.Components;

namespace ShadcnBlazor;
public class Dialog : ShadcnComponentBase
{
    public DialogContext Context { get; set; }
    public Dialog()
    {
        Context = new DialogContext(this);
    }
    readonly SemaphoreSlim _showLock = new SemaphoreSlim(1, 1);
    IDialogReference? _reference;

    /// <summary>
    /// Used to attach any user data object to the component.
    /// </summary>
    [Parameter]
    public object? Data { get; set; } = default!;
    /// <summary>
    /// The modality of the dialog. When set to true, interaction with outside elements will be disabled and only dialog content will be visible to screen readers.
    /// </summary>
    [Parameter]
    public bool Modal { get; set; }
    /// <summary>
    /// The event callback invoked to return the dialog result.
    /// </summary>
    [Parameter]
    public EventCallback<DialogResult> OnDialogResult { get; set; }
    [Parameter]
    public bool Open { get; set; }
    /// <summary>
    /// Event handler called when the open state of the dialog changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> OpenChanged { get; set; }
    /// <summary>
    /// Prevents scrolling outside of the dialog while it is shown.
    /// </summary>
    [Parameter]
    public bool PreventScroll { get; set; } = true;
    /// <summary>
    /// Gets or sets a value indicating whether that the dialog should trap focus.
    /// </summary>
    [Parameter]
    public bool? TrapFocus { get; set; }
    internal string OpenValueAsString => Open ? "open" : "closed";
    /// <summary>
    /// Gets True if the Dialog was called from the DialogService.
    /// </summary>
    bool CallingFromDialogService => ChildContent is null;
    [CascadingParameter]
    IDialogInstanceInternal? DialogInstance { get; set; }
    [Inject]
    IDialogService? DialogService { get; set; }
    bool IsInline => DialogInstance == null;

    /// <summary>
    /// For inlined dialogs, hides this dialog.
    /// </summary>
    /// <param name="result">The optional data to include.</param>
    public async Task CloseAsync(DialogResult? result = null)
    {
        if (!IsInline || _reference is null)
        {
            return;
        }
        Open = false;
        await OpenChanged.InvokeAsync(false);
        _reference.Close(result);
        _reference = null;
    }
    public async Task<IDialogReference?> ShowAsync()
    {
        await _showLock.WaitAsync();
        try
        {
            if (!IsInline)
            {
                throw new InvalidOperationException("You can only show an inlined dialog.");
            }

            if (_reference is not null)
            {
                return _reference;
            }

            var parameters = new DialogParameters
            {
                [nameof(ChildContent)] = ChildContent,
            };

            if (DialogService != null)
            {
                _reference = await DialogService.ShowAsync<Dialog>(parameters);

                // Do not await this!
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                _reference.Result.ContinueWith(t => InvokeAsync(() => OpenChanged.InvokeAsync(true)));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
        }
        finally
        {
            _showLock.Release();
        }
        return _reference;
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (IsInline)
        {
            if (Open && _reference is null)
            {
                await ShowAsync();
            }
            else if (_reference is not null)
            {
                if (Open)
                {
                    // Forward render update to instance
                    (_reference.Dialog as IStateHasChanged)?.StateHasChanged();
                }
                else
                {
                    // If we still have reference, but it's not visible call Close
                    await CloseAsync();
                }
            }
        }
        await base.OnAfterRenderAsync(firstRender);
    }
    protected override void OnInitialized()
    {
        base.OnInitialized();
        DialogInstance?.Register(this);
    }
}
