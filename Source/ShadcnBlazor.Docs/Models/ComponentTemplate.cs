namespace ShadcnBlazor.Docs;
public record ComponentTemplate
{
    public ComponentTemplate(Type type)
    {
        Type = type;
    }
    public ComponentTemplate(Type type, IDictionary<string, object> parameters) : this(type)
    {
        Parameters = parameters;
    }
    public Type Type { get; set; }
    public IDictionary<string, object>? Parameters { get; set; }
}
