namespace ShadcnBlazor.Docs;
public record SlotComponent
{
    public SlotComponent(Type type)
    {
        Type = type;
    }
    public SlotComponent(Type type, IDictionary<string, object> parameters) : this(type)
    {
        Parameters = parameters;
    }
    public Type Type { get; set; }
    public IDictionary<string, object>? Parameters { get; set; }
}
