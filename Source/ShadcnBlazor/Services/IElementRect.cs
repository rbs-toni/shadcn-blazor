using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Linq;

namespace ShadcnBlazor;
public interface IElementRect
{
    Task<ElementRect> GetBoundingClientRectAsync(ElementReference element);
}

public class ElementRectService : JSModule, IElementRect
{
    public ElementRectService(IJSRuntime js) : base(js, "./_content/ShadcnBlazor/modules/rect.min.js")
    {
    }
    public async Task<ElementRect> GetBoundingClientRectAsync(ElementReference element)
    {
        return await InvokeAsync<ElementRect>("getBoundingClientRect", element);
    }
}

public record ElementRect
{
    public double Height { get; set; }
    public double Width { get; set; }
    public double Top { get; set; }
    public double Left { get; set; }
}
