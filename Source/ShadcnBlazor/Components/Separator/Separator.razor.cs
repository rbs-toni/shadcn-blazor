using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using TailwindMerge;

namespace ShadcnBlazor;
public partial class Separator : ShadcnComponentBase
{
    /// <summary>
    /// Orientation of the component. Either `vertical` or `horizontal`. Defaults to `horizontal`.
    /// </summary>
    [Parameter]
    public Orientation Orientation { get; set; }

    /// <summary>
    /// Whether or not the component is purely decorative. When `true`, accessibility-related attributes are updated so that that the rendered element is removed from the accessibility tree.
    /// </summary>
    [Parameter]
    public bool Decorative { get; set; }

    [Parameter]
    public string? Label { get; set; }
}
