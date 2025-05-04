using System.Collections;

namespace ShadcnBlazor.Docs;
public record BreadcrumbItem
{
    public string? Title { get; init; }
    public string? Href { get; init; }
}

public abstract record BreadcrumbCollection : IEnumerable<BreadcrumbItem>
{
    protected readonly List<BreadcrumbItem> _items = [];

    public void Add(BreadcrumbItem item) => _items.Add(item);
    public void Add(string title, string href) => _items.Add(new BreadcrumbItem()
    {
        Title = title,
        Href = href,
    });
    public void AddRange(IEnumerable<BreadcrumbItem> items) => _items.AddRange(items);
    public void Clear() => _items.Clear();
    public IEnumerator<BreadcrumbItem> GetEnumerator() => _items.GetEnumerator();

    // Makes it work with foreach
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    // Common properties
    public int Count => _items.Count;
    public List<BreadcrumbItem> Items => _items;
    public BreadcrumbItem this[int index] => _items[index];
}

public record DocsBreadcrumbCollection : BreadcrumbCollection
{
    public DocsBreadcrumbCollection(string title)
    {
        Add("Docs", "/docs");
        Add(title, "");
    }
}

// This now works exactly as you wanted
public record ComponentsBreadcrumbCollection : BreadcrumbCollection
{
    public ComponentsBreadcrumbCollection(string title)
    {
        Add("Docs", "/docs");
        Add("Components", "/docs/components");
        Add(title, "");
    }
}