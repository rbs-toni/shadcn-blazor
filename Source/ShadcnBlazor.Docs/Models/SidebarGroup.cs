using System;
namespace ShadcnBlazor.Docs;

/// <summary>
/// Represents a group of navigation items in the sidebar.
/// </summary>
public class SidebarGroup
{
    /// <summary>
    /// Gets or sets the display name of the sidebar group.
    /// </summary>
    public string? Name { get; set; }
    public int Order { get; set; }
    /// <summary>
    /// Gets or sets the collection of sidebar items within this group.
    /// </summary>
    public List<SidebarItem>? Items { get; set; }
}
