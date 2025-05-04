namespace ShadcnBlazor.Docs;
public class Header
{
    public string? Level { get; set; }
    public string? Title { get; set; }
    public string? Href { get; set; }
    public List<Header>? Items { get; set; }
    public virtual bool Equals(Header? other)
    {
        if (other is null)
        {
            return false;
        }

        if (Level != other.Level ||
            Title != other.Title ||
            Href != other.Href ||
            (Items?.Count ?? 0) != (other.Items?.Count ?? 0))
        {
            return false;
        }

        if (Items is not null &&
            Items.Count > 0)
        {
            for (var i = 0; i < Items.Count; i++)
            {
                if (!Items[i].Equals(other.Items![i]))
                {
                    return false;
                }
            }
        }

        return true;
    }

    public override int GetHashCode()
         => HashCode.Combine(Level, Title, Href);
}
