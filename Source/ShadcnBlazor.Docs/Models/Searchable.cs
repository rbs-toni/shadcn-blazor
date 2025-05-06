namespace ShadcnBlazor.Docs;
/// <summary>
/// Represents a searchable content entity with keyword metadata for filtering or discovery.
/// </summary>
public abstract class Searchable
{
    /// <summary>
    /// Gets or sets the list of keyword labels associated with this item.
    /// </summary>
    public List<string> Keywords { get; set; } = [];

    /// <summary>
    /// Gets the normalized list of keywords (trimmed, lowercased, no duplicates).
    /// </summary>
    public IReadOnlyList<string> NormalizedKeywords =>
        Keywords
            .Select(k => k.Trim().ToLowerInvariant())
            .Where(k => !string.IsNullOrWhiteSpace(k))
            .Distinct()
            .ToList();
}
