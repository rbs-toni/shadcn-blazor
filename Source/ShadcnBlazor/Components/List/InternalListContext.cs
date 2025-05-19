using Microsoft.AspNetCore.Components;

namespace ShadcnBlazor;

// The list cascades this so that descendant options can talk back to it.
// It's an internal type so it doesn't show up in unrelated components by mistake.
class InternalListContext<TOption>(ListComponentBase<TOption> listComponent) where TOption : notnull
{

    readonly ICollection<ListOption<TOption>> options = [];

    public ListComponentBase<TOption> ListComponent { get; } = listComponent;

    /// <summary>
    /// Gets the list of all select items inside of this select component.
    /// </summary>
    public ICollection<ListOption<TOption>> Options => options;

    internal void Register(ListOption<TOption> option)
    {
        if (option is null)
        {
            return;
        }

        if (!options.Contains(option))
        {
            options.Add(option);
        }
    }

    internal void Unregister(ListOption<TOption> option)
    {
        if (option is null)
        {
            return;
        }

        options.Remove(option);
    }

    /// <summary>
    /// Gets the event callback to be invoked when the selected value is changed.
    /// </summary>
    public EventCallback<string?> ValueChanged { get; set; }

    /// <summary>
    /// Gets the event callback to be invoked when the selected value is changed.
    /// </summary>
    public EventCallback<TOption?> SelectedOptionChanged { get; set; }
}

