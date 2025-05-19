using Microsoft.AspNetCore.Components;
using System;
using System.Linq;

namespace ShadcnBlazor;
public class PopperContentContext
{
    private readonly PopperContent _popperContent;

    public PopperContentContext(PopperContent popperContent)
    {
        _popperContent = popperContent;
    }
    public event Action<ElementReference>? OnArrowChange;

    public double ArrowX { get; set; }
    public double ArrowY { get; set; }
    public bool ShouldHideArrow { get; set; }
    public PopperSide Side { get; set; }

    public void NotifyArrowChange(ElementReference arrow)
    {
        OnArrowChange?.Invoke(arrow);
    }
}
