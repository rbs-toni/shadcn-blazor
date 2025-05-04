using ShadcnBlazor.Docs.Enums;

namespace ShadcnBlazor.Docs;
public abstract class Linkable
{
    public string? Href { get; init; }
    public string? Title { get; init; }
    public Target? Target { get; init; }
    public Rel? Rel { get; set; }
}
