namespace ShadcnBlazor;

public record ToastClasses
{
    /// <summary>The main container class for the toast.</summary>
    public string? Toast { get; set; }

    /// <summary>The class for the toast title.</summary>
    public string? Title { get; set; }

    /// <summary>The class for the toast description text.</summary>
    public string? Description { get; set; }

    /// <summary>The class for the toast loader/progress bar.</summary>
    public string? Loader { get; set; }

    /// <summary>The class for the close (×) button.</summary>
    public string? CloseButton { get; set; }

    /// <summary>The class for the cancel button.</summary>
    public string? CancelButton { get; set; }

    /// <summary>The class for the primary action button.</summary>
    public string? ActionButton { get; set; }

    /// <summary>The class to apply when toast type is success.</summary>
    public string? Success { get; set; }

    /// <summary>The class to apply when toast type is error.</summary>
    public string? Error { get; set; }

    /// <summary>The class to apply when toast type is informational.</summary>
    public string? Info { get; set; }

    /// <summary>The class to apply when toast type is warning.</summary>
    public string? Warning { get; set; }

    /// <summary>The class to apply when toast is loading.</summary>
    public string? Loading { get; set; }

    /// <summary>The default fallback class for the toast.</summary>
    public string? Default { get; set; }

    /// <summary>The class for the content wrapper of the toast.</summary>
    public string? Content { get; set; }

    /// <summary>The class for the icon within the toast.</summary>
    public string? Icon { get; set; }

    public string? GetToastClassBasedOnType(ToastType? toastType)
    {
        return toastType switch
        {
            ToastType.Success => Success,
            ToastType.Error => Error,
            ToastType.Info => Info,
            ToastType.Warning => Warning,
            ToastType.Loading => Loading,
            ToastType.Action => ActionButton,
            ToastType.Default => Default,
            ToastType.Normal => Toast,
            _ => Default
        };
    }
}
