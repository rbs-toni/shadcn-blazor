using Microsoft.AspNetCore.Components;
using System;
using System.Linq;

namespace ShadcnBlazor;
public class PopperContentContext
{
    readonly PopperContent _popperContent;
    ElementReference _arrow;

    public PopperContentContext(PopperContent popperContent)
    {
        _popperContent = popperContent;
    }

    public event Action? OnArrowChange;

    public ElementReference Arrow => _arrow;
    public double ArrowX => _popperContent.ArrowX;
    public double ArrowY => _popperContent.ArrowY;
    public bool ShouldHideArrow { get; set; }
    public PopperSide Side { get; set; }

    public void SetArrow(ElementReference arrow)
    {
        _arrow = arrow;
        OnArrowChange?.Invoke();
    }
}
