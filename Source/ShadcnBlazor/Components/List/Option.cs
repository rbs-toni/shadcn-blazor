namespace ShadcnBlazor;

public class Option<TType> : IOptionIcon
{
    public TType? Value { get; set; }

    public string? Text { get; set; }

    public (IconName Name, string Class)? Icon { get; set; }

    public bool Disabled { get; set; } = false;

    public bool Selected { get; set; } = false;
}
