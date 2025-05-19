namespace ShadcnBlazor;

public record ToastToDismiss
{
    /// <summary>
    /// The identifier of the toast, which can be numeric or string-based.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Whether the toast should be dismissed.
    /// </summary>
    public bool Dismiss { get; set; }
}
