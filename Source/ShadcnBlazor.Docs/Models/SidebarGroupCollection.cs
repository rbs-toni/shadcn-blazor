using System.Collections;

namespace ShadcnBlazor.Docs;

/// <summary>
/// Represents a collection of sidebar groups with unique names and preserved insertion order.
/// </summary>
public class SidebarGroupCollection : IEnumerable<SidebarGroup>
{
    private readonly List<SidebarGroup> _items = new();
    private readonly HashSet<string> _names = new();

    /// <summary>
    /// Adds a sidebar group to the collection. Name must be unique.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown if the name is null, empty, or already exists.</exception>
    public void Add(SidebarGroup sidebar)
    {
        if (string.IsNullOrWhiteSpace(sidebar.Title))
            throw new ArgumentException("Sidebar name must be non-null and non-empty");

        if (!_names.Add(sidebar.Title))
            throw new ArgumentException($"Sidebar with name '{sidebar.Title}' already exists");

        _items.Add(sidebar);
    }

    /// <summary>
    /// Gets the sidebar group at the specified index.
    /// </summary>
    public SidebarGroup this[int index] => _items[index];

    /// <summary>
    /// Gets the number of sidebar groups in the collection.
    /// </summary>
    public int Count => _items.Count;

    public IEnumerator<SidebarGroup> GetEnumerator() => _items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

