using ShadcnBlazor.Docs;

/// <summary>
/// Manages layout state for the application
/// </summary>
public sealed class LayoutContext
{
    List<Header> _headers = [];
    List<SidebarGroup> _sidebar;
    bool _useOutline;
    bool _useSidebar;
    bool _useFooterLinks = true;
    string? _pageTitle;
    /// <summary>
    /// Creates a new layout context with the required sidebar groups
    /// </summary>
    public LayoutContext(List<SidebarGroup> sidebarGroups, bool useSidebar = false)
    {
        _sidebar = sidebarGroups ?? throw new ArgumentNullException(nameof(sidebarGroups));
        _useSidebar = useSidebar;
    }

    public event Action? OnChange;

    public bool HasOutline => _useOutline && _headers.Count > 0;
    public bool HasSidebar => _useSidebar && _sidebar.Count > 0;
    public List<Header> Headers => _headers;
    public List<SidebarGroup> Sidebar => _sidebar;
    public string? PageTitle => _pageTitle;
    public bool UseOutline => _useOutline;
    public bool UseSidebar => _useSidebar;
    public bool UseFooterLinks => _useFooterLinks;

    public void SetLayoutOptions(string pageTitle, bool useSidebar, bool useOutline, bool useFooterLinks)
    {
        _pageTitle = pageTitle;
        _useSidebar = useSidebar;
        _useOutline = useOutline;
        _useFooterLinks = useFooterLinks;
        OnChange?.Invoke();
    }
    public void SetHeaders(List<Header> headers, bool notify = true)
    {
        _headers = headers ?? [];
        if (notify)
        {
            OnChange?.Invoke();
        }
    }
    public void SetSidebar(List<SidebarGroup> sidebarGroups, bool notify = true)
    {
        _sidebar = sidebarGroups ?? throw new ArgumentNullException(nameof(sidebarGroups));
        if (notify)
        {
            OnChange?.Invoke();
        }
    }
}
