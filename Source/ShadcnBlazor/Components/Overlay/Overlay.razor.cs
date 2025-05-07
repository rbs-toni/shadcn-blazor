// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace ShadcnBlazor;
public partial class Overlay : IAsyncDisposable
{
    const string JSFile = "./_content/ShadcnBlazor/Components/Overlay/Overlay.razor.js";
    readonly string _defaultId = Identifier.NewId();
    DotNetObjectReference<Overlay>? _dotNetHelper;
    IJSObjectReference? _jsModule;

    /// <summary>
    /// Gets of sets a value indicating if the overlay can be dismissed by clicking on it.
    /// Default is true.
    /// </summary>
    [Parameter]
    public bool Dismissable { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the overlay is shown full screen or bound to the containing element.
    /// </summary>
    [Parameter]
    public bool FullScreen { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether the overlay is interactive, except for the element with the specified <see cref="InteractiveExceptId"/>.
    /// In other words, the elements below the overlay remain usable (mouse-over, click) and the overlay will closed when clicked.
    /// </summary>
    [Parameter]
    public bool Interactive { get; set; }
    /// <summary>
    /// Gets or sets the HTML identifier of the element that is not interactive when the overlay is shown.
    /// This property is ignored if <see cref="Interactive"/> is false.
    /// </summary>
    [Parameter]
    public string? InteractiveExceptId { get; set; }
    /// <summary>
    /// Callback for when the overlay is closed.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClose { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether the overlay is visible.
    /// </summary>
    [Parameter]
    public bool Open { get; set; }
    /// <summary>
    /// Callback for when overlay visibility changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> OpenChanged { get; set; }
    [Parameter]
    public bool PreventScroll { get; set; }
    /// <summary>
    /// Gets or set if the overlay is transparent.
    /// </summary>
    [Parameter]
    public bool Transparent { get; set; } = true;
    [Inject]
    IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary>
    /// Disposes the overlay.
    /// </summary>
    /// <returns></returns>
    public async ValueTask DisposeAsync()
    {
        await InvokeOverlayDisposeAsync();

        if (_jsModule != null)
        {
            await _jsModule.DisposeAsync();
        }
    }
    public async Task OnCloseHandlerAsync(MouseEventArgs e)
    {
        if (!Dismissable || !Open || Interactive)
        {
            return;
        }

        // Close the overlay
        await OnCloseInternalHandlerAsync(e);
    }
    [JSInvokable]
    public async Task OnCloseInteractiveAsync(MouseEventArgs e)
    {
        if (!Dismissable || !Open)
        {
            return;
        }

        // Remove the document.removeEventListener
        await InvokeOverlayDisposeAsync();

        // Close the overlay
        await OnCloseInternalHandlerAsync(e);
    }
    protected override async Task OnParametersSetAsync()
    {
        if (Interactive)
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = _defaultId;
            }

            if (Open)
            {
                // Add a document.addEventListener when Open is true
                await InvokeOverlayInitializeAsync();
            }
            else
            {
                // Remove a document.addEventListener when Open is false
                await InvokeOverlayDisposeAsync();
            }
        }
    }
    async Task InvokeOverlayDisposeAsync()
    {
        if (_jsModule != null && Interactive)
        {
            await _jsModule.InvokeVoidAsync("overlayDispose", InteractiveExceptId);
        }
    }
    async Task InvokeOverlayInitializeAsync()
    {
        _dotNetHelper ??= DotNetObjectReference.Create(this);
        _jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JSFile);

        var containerId = FullScreen ? null : Id;
        await _jsModule.InvokeVoidAsync("overlayInitialize", _dotNetHelper, containerId, InteractiveExceptId);
    }
    async Task OnCloseInternalHandlerAsync(MouseEventArgs e)
    {
        Open = false;

        if (OpenChanged.HasDelegate)
        {
            await OpenChanged.InvokeAsync(Open);
        }

        if (OnClose.HasDelegate)
        {
            await OnClose.InvokeAsync(e);
        }
    }
}
