using Microsoft.AspNetCore.Components.Routing;

namespace ShadcnBlazor.Docs;

/// <summary>
/// Represents an individual navigation item in the sidebar.
/// </summary>
public class SidebarItem : Linkable
{
    /// <summary>
    /// Gets or sets the icon associated with this sidebar item.
    /// </summary>
    public IconName? Icon { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this item is currently active/selected.
    /// </summary>
    public NavLinkMatch Match { get; set; }

    public int Order { get; set; }

    public List<string>? Matches { get; set; }

    /// <summary>
    /// Gets or sets the nested collection of sidebar items (for hierarchical navigation).
    /// </summary>
    public List<SidebarItem>? Items { get; set; }
}
