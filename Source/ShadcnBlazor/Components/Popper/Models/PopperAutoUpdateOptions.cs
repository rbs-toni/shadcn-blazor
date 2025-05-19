namespace ShadcnBlazor;
public record PopperAutoUpdateOptions
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
