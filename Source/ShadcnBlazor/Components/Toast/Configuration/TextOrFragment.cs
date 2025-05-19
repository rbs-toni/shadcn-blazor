using Microsoft.AspNetCore.Components;

namespace ShadcnBlazor;
public readonly struct TextOrFragment
{
    public string? Label { get; }
    public RenderFragment? Fragment { get; }

    TextOrFragment(string? label, RenderFragment? fragment)
    {
        Label = label;
        Fragment = fragment;
    }

    public static implicit operator TextOrFragment(string label) => new(label, null);
    public static implicit operator TextOrFragment(RenderFragment fragment) => new(null, fragment);
    public bool IsString => Label != null;
    public bool IsFragment => Fragment != null;
}
