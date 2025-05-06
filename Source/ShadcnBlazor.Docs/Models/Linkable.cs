using Microsoft.AspNetCore.Components.Routing;
using ShadcnBlazor.Docs.Enums;

namespace ShadcnBlazor.Docs;

/// <summary>
/// Represents a searchable linkable entity, such as a documentation entry or sitemap item.
/// </summary>
public abstract class Linkable : Searchable
{
    /// <summary>
    /// Gets or sets the hyperlink reference (URL or route) for this item.
    /// </summary>
    public string? Href { get; set; }

    /// <summary>
    /// Gets or sets the display title of the item.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the target attribute indicating how the link should be opened.
    /// </summary>
    public Target? Target { get; set; }

    /// <summary>
    /// Gets or sets the relationship between the current page and the linked one.
    /// </summary>
    public Rel? Rel { get; set; }

    /// <summary>
    /// Gets or sets a value indicating how the link matching is evaluated for activation.
    /// </summary>
    public NavLinkMatch Match { get; set; } = NavLinkMatch.All;

    /// <summary>
    /// Gets or sets the display order of this sidebar item within its group.
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// Gets or sets the list of additional URL segments or identifiers this item should match against.
    /// Useful for custom highlighting or route aliasing.
    /// </summary>
    public List<string> Matches { get; set; } = new();

    public bool Disabled { get; set; }
}
