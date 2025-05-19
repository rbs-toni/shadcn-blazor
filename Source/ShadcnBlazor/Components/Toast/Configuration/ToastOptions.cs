namespace ShadcnBlazor;
public class ToastOptions
{
    /// <summary>
    /// The root CSS class for the toast.
    /// </summary>
    public string? Class { get; set; }

    /// <summary>
    /// Whether to show a close button.
    /// </summary>
    public bool? CloseButton { get; set; }

    /// <summary>
    /// Custom CSS class for the toast description.
    /// </summary>
    public string? DescriptionClass { get; set; }

    /// <summary>
    /// Inline styles for the toast container.
    /// </summary>
    public string? Style { get; set; }

    /// <summary>
    /// Inline styles for the cancel button.
    /// </summary>
    public string? CancelButtonStyle { get; set; }

    /// <summary>
    /// Inline styles for the action button.
    /// </summary>
    public string? ActionButtonStyle { get; set; }

    /// <summary>
    /// Duration in milliseconds before the toast is dismissed.
    /// </summary>
    public int? Duration { get; set; }

    /// <summary>
    /// Whether to skip default styling (unstyled mode).
    /// </summary>
    public bool? Unstyled { get; set; }

    /// <summary>
    /// Custom CSS class mapping for toast parts.
    /// </summary>
    public ToastClasses? Classes { get; set; }
}
