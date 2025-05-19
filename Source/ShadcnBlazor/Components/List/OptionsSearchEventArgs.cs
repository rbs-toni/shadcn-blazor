namespace ShadcnBlazor;

/// <summary>
/// <see cref="Autocomplete{TOption}"/> uses this event to return the list of items to display.
/// </summary>
/// <typeparam name="T"></typeparam>
public class OptionsSearchEventArgs<T>
{
    /// <summary>
    /// Gets or sets the text to search.
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of items to display.
    /// </summary>
    public IEnumerable<T>? Items { get; set; }
}
