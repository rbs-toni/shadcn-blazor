using System.ComponentModel;

namespace ShadcnBlazor.Docs.Enums;

/// <summary>
/// Specifies where to open the linked document (corresponds to HTML anchor target attribute)
/// </summary>
public enum Target
{
    /// <summary>
    /// Opens the link in the same frame (default behavior)
    /// </summary>
    [Description("_self")]
    Self,

    /// <summary>
    /// Opens the link in a new window or tab
    /// </summary>
    [Description("_blank")]
    Blank,

    /// <summary>
    /// Opens the link in the parent frame
    /// </summary>
    [Description("_parent")]
    Parent,

    /// <summary>
    /// Opens the link in the full body of the window
    /// </summary>
    [Description("_top")]
    Top
}
