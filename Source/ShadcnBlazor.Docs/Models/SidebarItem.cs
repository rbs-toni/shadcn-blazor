namespace ShadcnBlazor.Docs;
/// <summary>
/// Represents an individual navigation item in the documentation sidebar.
/// </summary>
public class SidebarItem : Linkable, IProgressable
{
    /// <summary>
    /// Gets or sets the icon associated with this sidebar item.
    /// </summary>
    public IconName? Icon { get; set; }

    /// <summary>
    /// Gets or sets the nested collection of sidebar items for hierarchical navigation.
    /// </summary>
    public List<SidebarItem>? Items { get; set; }
    public ProgressState ProgressState { get; set; } = ProgressState.NotStarted;

    public bool IsValidLink => !string.IsNullOrWhiteSpace(Href);

    public string? Label => GetProgressStateLabel();

    string? GetProgressStateLabel()
    {
        return ProgressState switch
        {
            ProgressState.InProgress => "WIP",
            _ => default,
        };
    }
}
