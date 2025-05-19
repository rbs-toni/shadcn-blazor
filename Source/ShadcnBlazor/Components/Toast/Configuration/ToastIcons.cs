using Microsoft.AspNetCore.Components;

namespace ShadcnBlazor;
public class ToastIcons
{
    public RenderFragment? Success { get; set; }
    public RenderFragment? Info { get; set; }
    public RenderFragment? Warning { get; set; }
    public RenderFragment? Error { get; set; }
    public RenderFragment? Loading { get; set; }
    public RenderFragment? Close { get; set; }
}
