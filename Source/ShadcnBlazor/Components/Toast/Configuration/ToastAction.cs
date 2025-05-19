using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace ShadcnBlazor;
public class ToastAction
{
    /// <summary>
    /// The label content, which can be a string or a component.
    /// </summary>
    public RenderFragment? Label { get; set; }

    /// <summary>
    /// The callback triggered when the action is clicked.
    /// </summary>
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    /// <summary>
    /// Optional inline styles for the action button, as a dictionary of CSS properties.
    /// </summary>
    public string? ActionButtonStyle { get; set; }
}
