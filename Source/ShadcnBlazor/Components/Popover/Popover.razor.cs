using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ShadcnBlazor;
public partial class Popover : ShadcnJSComponentBase
{
    public Popover() : base("Popover/Popover")
    {
    }
    Floating _floating = default!;

    /// <summary>
    /// Gets or sets the id of the component the popover is positioned relative to.
    /// </summary>
    [Parameter]
    public string AnchorId { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets popover opened state.
    /// </summary>
    [Parameter]
    public bool Open { get; set; }

    /// <summary>
    /// Callback for when open state changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> OpenChanged { get; set; }

    /// <summary>
    /// Gets or sets whether the element should receive the focus when the component is loaded.
    /// If this is the case, the user cannot navigate to other elements of the page while the Popup is open.
    /// Default is true.
    /// </summary>
    [Parameter]
    public bool AutoFocus { get; set; } = true;

    /// <summary>
    /// Gets or sets the keys that can be used to close the popover.
    /// By default, Escape
    /// </summary>
    [Parameter]
    public KeyCode[]? CloseKeys { get; set; } = [KeyCode.Escape];

    KeyCode[] CloseAndTabKeys => CloseKeys?.Any() == true ? CloseKeys.Union([KeyCode.Tab]).ToArray() : [KeyCode.Tab];

    protected override void OnInitialized()
    {
        if (CloseKeys != null && CloseKeys.Any() && string.IsNullOrEmpty(Id))
        {
            Id = Identifier.NewId();
        }
    }

    protected virtual async Task CloseAsync()
    {
        Open = false;
        if (OpenChanged.HasDelegate)
        {
            await OpenChanged.InvokeAsync(Open);
        }
        //await Floating.FocusToOriginalElementAsync();
    }

    protected virtual async Task CloseOnKeyAsync(KeyCodeEventArgs e)
    {
        if (CloseKeys != null && CloseKeys.Contains(e.Key))
        {
            await CloseAsync();
        }

        if (AutoFocus && e.Key == KeyCode.Tab)
        {
            //await Floating.FocusToNextElementAsync();
        }
    }
    [JSInvokable]
    public async Task OnCloseFromJSAsync()
    {
        await CloseAsync();
    }
    DotNetObjectReference<Popover>? _dotNetObject;
    protected override async Task OnParametersSetAsync()
    {
        if (Open)
        {
            _dotNetObject ??= DotNetObjectReference.Create(this);
            await InvokeVoidAsync("init", Id, _dotNetObject, nameof(OnCloseFromJSAsync));
        }
        else
        {
            await InvokeVoidAsync("dispose", Id);
        }
    }
}
