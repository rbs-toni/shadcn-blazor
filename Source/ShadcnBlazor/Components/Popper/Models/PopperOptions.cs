using Microsoft.AspNetCore.Components;

namespace ShadcnBlazor;

public record PopperOptions
{
    public int SideOffset { get; set; }
    public int AlignOffset { get; set; }
    public string? Placement { get; set; }
    public string? Strategy { get; set; }
    public ElementReference? Arrow { get; set; }
}
