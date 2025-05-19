using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Linq;

namespace ShadcnBlazor;
public partial class ResizableHandle : IAsyncDisposable
{
    const string JF_FILE = "./_content/ShadcnBlazor/Components/Resizable/ResizableHandle.razor.js";
    DotNetObjectReference<ResizableHandle>? _dotNetObject;
    bool _isFocused;
    IJSObjectReference? _jsModule;
    IJSObjectReference? _resizeHandlerReference;
    ResizeHandleState _resizeHandleState = ResizeHandleState.Inactive;

    public ResizableHandle()
    {
        Id = Identifier.NewId();
    }

    [Inject]
    IJSRuntime JSRuntime { get; set; } = default!;
    ResizeDirection Direction => GroupContext?.Direction ?? ResizeDirection.Horizontal;
    string? GroupId => GroupContext?.GroupId;
    async Task StartDragging(string dragHandleId, ResizeEventData eventData)
    {
        if (GroupContext != null)
        {
            await GroupContext.StartDragging(dragHandleId, eventData);
        }
    }
    async Task StopDragging()
    {
        if (GroupContext != null)
        {
            await GroupContext.StopDragging();
        }
    }
    ElementReference? PanelGroupElement => GroupContext?.PanelGroupElement;
    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_resizeHandlerReference != null)
            {
                await _resizeHandlerReference.InvokeVoidAsync("dispose");
                await _resizeHandlerReference.DisposeAsync();
            }

            _dotNetObject?.Dispose();

            if (_jsModule != null)
            {
                await _jsModule.DisposeAsync();
            }
        }
        catch (JSDisconnectedException)
        {
            // Handle if JS is no longer available
        }
    }
    [JSInvokable]
    public async Task HandleResizeEventAsync(string action, bool isActive, ResizeEventData eventData)
    {
        if (Disabled)
        {
            return;
        }

        switch (action)
        {
            case "down":
                Console.WriteLine("down");
                _resizeHandleState = ResizeHandleState.Drag;
                await OnDragging.InvokeAsync(true);
                if (GroupContext != null)
                {
                    await GroupContext.StartDragging(Id, eventData);
                }
                break;

            case "move":
                Console.WriteLine("down");
                if (_resizeHandleState != ResizeHandleState.Drag)
                {
                    _resizeHandleState = ResizeHandleState.Hover;
                }

                if (GroupContext != null && isActive)
                {
                    // Calculate delta based on direction
                    double delta;
                    if (GroupContext.Direction == ResizeDirection.Horizontal)
                    {
                        delta = eventData.ClientX - GroupContext.DragState.InitialCursorPosition;
                    }
                    else
                    {
                        delta = eventData.ClientY - GroupContext.DragState.InitialCursorPosition;
                    }

                    // Check for RTL if horizontal
                    if (GroupContext.Direction == ResizeDirection.Horizontal)
                    {
                        var isRtl = await JSRuntime.InvokeAsync<bool>("isRtl", GroupContext.PanelGroupElement);
                        if (isRtl)
                        {
                            delta = -delta;
                        }
                    }

                    // Get the resize handler function from the context
                    var resizeHandler = GroupContext.RegisterResizeHandle(Id);
                    if (resizeHandler != null)
                    {
                        // Create a ResizeEvent that the handler expects
                        var resizeEvent = new ResizeEventData
                        {
                            ClientX = eventData.ClientX,
                            ClientY = eventData.ClientY,
                        };

                        // Invoke the resize handler
                        resizeHandler(resizeEvent);
                    }
                }
                break;

            case "up":
                Console.WriteLine("up");
                _resizeHandleState = ResizeHandleState.Inactive;
                await OnDragging.InvokeAsync(false);
                if (GroupContext != null)
                {
                    await GroupContext.StopDragging();
                }
                break;
        }

        StateHasChanged();
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && !Disabled)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>(
                "import", JF_FILE);

            _dotNetObject ??= DotNetObjectReference.Create(this);

            _resizeHandlerReference = await _jsModule.InvokeAsync<IJSObjectReference>(
                "registerResizeHandle",
                Id,
                Ref,
                Direction.ToStringFast(),
                HitAreaMargins,
                _dotNetObject,
                nameof(HandleResizeEventAsync));
        }
    }
    protected override void OnInitialized()
    {
        if (GroupContext == null)
        {
            throw new InvalidOperationException(
               "ResizableHandle components must be rendered within a ResizablePanelGroup component");
        }
    }
    void HandleBlur(FocusEventArgs e)
    {
        _isFocused = false;
        StateHasChanged();
    }
    void HandleFocus(FocusEventArgs e)
    {
        _isFocused = true;
        StateHasChanged();
    }
}
