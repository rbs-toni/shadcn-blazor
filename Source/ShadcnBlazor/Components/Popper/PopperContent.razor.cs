using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Linq;

namespace ShadcnBlazor;
public partial class PopperContent : IAsyncDisposable
{
    readonly PopperContentContext _context;
    ElementReference _floatingRef;

    public PopperContent(PopperContentContext context)
    {
        _context = context;
    }

    [Inject]
    IJSRuntime JSRuntime { get; set; } = default!;

    IJSObjectReference? JSModule { get; set; }
    IJSObjectReference? FloatingInstance { get; set; }
    DotNetObjectReference<PopperContent>? DotNetRef { get; set; }
    const string JS_FILE = "./_content/ShadcnBlazor/Components/Popper/PopperContent.razor.js";
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            JSModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JS_FILE);
            if (JSModule != null)
            {
                DotNetRef ??= DotNetObjectReference.Create(this);
                FloatingInstance = await JSModule.InvokeAsync<IJSObjectReference>("create", DotNetRef, _floatingRef, Ref, _context.Side.ToString(), _context.ShouldHideArrow);
            }
        }
    }

    public async ValueTask DisposeAsync()
    {
        DotNetRef?.Dispose();
        if (JSModule!=null)
        {
            await JSModule.DisposeAsync();
        }
        if (FloatingInstance != null)
        {
            await FloatingInstance.InvokeVoidAsync("cleanUp");
            await FloatingInstance.DisposeAsync();
        }
    }
}

public class PopperAutoUpdateOptions
{
    /// <summary>
    /// Whether to update the position when an overflow ancestor is scrolled.
    /// </summary>
    public bool AncestorScroll { get; set; } = true;

    /// <summary>
    /// Whether to update the position when an overflow ancestor is resized. This uses the resize event.
    /// </summary>
    public bool AncestorResize { get; set; } = true;

    /// <summary>
    /// Whether to update the position when either the reference or floating elements resized. This uses a ResizeObserver.
    /// </summary>
    public bool ElementResize { get; set; } = true;

    /// <summary>
    /// Whether to update the position of the floating element if the reference element moved on the screen as the result of layout shift.
    /// </summary>
    public bool LayoutShift { get; set; } = true;

    /// <summary>
    /// Whether to update the position of the floating element on every animation frame if required.
    /// </summary>
    public bool AnimationFrame { get; set; }
}
