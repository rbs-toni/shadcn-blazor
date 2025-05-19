using Microsoft.AspNetCore.Components;
using System;
using System.Linq;

namespace ShadcnBlazor;
public class PopperRootContext
{
    readonly PopperRoot _popper;

    public PopperRootContext(PopperRoot popper)
    {
        _popper = popper;
    }

    public event Action? OnAnchorChanged;

    public ElementReference Anchor { get; set; }

    public void SetAnchor(ElementReference anchord)
    {
        Anchor = anchord;
        OnAnchorChanged?.Invoke();
    }
}
