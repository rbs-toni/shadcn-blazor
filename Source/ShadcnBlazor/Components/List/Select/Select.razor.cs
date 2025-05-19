using Microsoft.AspNetCore.Components;

namespace ShadcnBlazor;
[CascadingTypeParameter(nameof(TOption))]
public partial class Select<TOption> : ListComponentBase<TOption> where TOption : notnull
{
    /// <summary>
    /// Gets the `Required` aria label.
    /// </summary>
    public static string RequiredAriaLabel = "Required";

    /// <summary>
    /// Gets or sets the open attribute.
    /// </summary>
    [Parameter]
    public bool? Open { get; set; }
    protected override string? StyleValue => new StyleBuilder(base.StyleValue)
        .AddStyle("min-width", Width, when: !string.IsNullOrEmpty(Width))
        .Build();
}
