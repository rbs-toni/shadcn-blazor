using Microsoft.JSInterop;

namespace ShadcnBlazor;
public partial class Popover : ShadcnJSComponentBase
{
    readonly PopoverContext _context;
    DotNetObjectReference<Popover>? _dotNetObject;
    bool _open;
    bool _shouldTeleported;
    string? _side;
    bool _teleported;

    public Popover() : base("Popover/Popover")
    {
        Id = Identifier.NewId();
        _context = new(this);
    }
    KeyCode[] CloseAndTabKeys => CloseKeys?.Any() == true ? CloseKeys.Union([KeyCode.Tab]).ToArray() : [KeyCode.Tab];

    [JSInvokable]
    public async Task OnCloseFromJSAsync()
    {
        await CloseAsync();
    }
    protected async Task CloseAsync()
    {
        if (_teleported)
        {
            _teleported = false;
            _context.Close();
            await Task.CompletedTask;
        }
    }
    protected async Task CloseOnKeyAsync(KeyCodeEventArgs e)
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
    protected override async ValueTask OnDisposingAsync()
    {
        _dotNetObject?.Dispose();
        await InvokeVoidAsync("dispose", Id);
    }
    protected override async Task OnParametersSetAsync()
    {
        if (Open)
        {
            _dotNetObject ??= DotNetObjectReference.Create(this);
            await InvokeVoidAsync("init", Id, Modal, _dotNetObject, nameof(OnCloseFromJSAsync));
        }
        else
        {
            await InvokeVoidAsync("dispose", Id);
        }
    }
    void OnAnimationEnd()
    {
        _shouldTeleported = false;
    }
    void OnPositionChanged(PositionChangedEventArgs args)
    {
        var side = args.Placement?.Split('-')[0];
        if (_side != side)
        {
            _side = side;
        }
    }
    void OnRemoved()
    {
        Open = false;
        OpenChanged.InvokeAsync(Open);
    }
    void OnTeleported()
    {
        if (!_teleported)
        {
            _teleported = true;
            InvokeAsync(StateHasChanged);
        }
    }
}
