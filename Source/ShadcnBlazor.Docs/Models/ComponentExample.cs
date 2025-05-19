namespace ShadcnBlazor.Docs;

public record ComponentExample
{
    public ComponentExample(string name, string title, ComponentTemplate template)
    {
        Name = name;
        Title = title;
        Template = template;
    }
    public string Name { get; init; }
    public string Title { get; init; }
    public ComponentTemplate Template { get; init; }
    public string CodePath => $"{Template.Type.Name}.razor";
}
