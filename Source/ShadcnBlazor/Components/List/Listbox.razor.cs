using Microsoft.AspNetCore.Components;

namespace ShadcnBlazor;

[CascadingTypeParameter(nameof(TOption))]
public partial class Listbox<TOption> : ListComponentBase<TOption> where TOption : notnull
{
    /// <summary>
    /// Gets or sets the maximum number of options that should be visible in the listbox scroll area.
    /// </summary>
    [Parameter]
    public int Size { get; set; }

    /// <summary />
    protected virtual StyleBuilder BorderStyle => new StyleBuilder()
        .AddStyle("width", Width, when: !string.IsNullOrEmpty(Width))
        .AddStyle("height", Height, when: !string.IsNullOrEmpty(Height))
        .AddStyle("overflow-y", "auto")
        .AddStyle("border", "calc(var(--stroke-width) * 1px) solid var(--neutral-stroke-rest)")
        .AddStyle("border-radius", "calc(var(--control-corner-radius) * 1px)");
}
