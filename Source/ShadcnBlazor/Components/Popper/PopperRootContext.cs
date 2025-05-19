using Microsoft.AspNetCore.Components;
using System;
using System.Linq;

namespace ShadcnBlazor;
public class PopperRootContext
{
    private readonly PopperRoot _popper;

    public PopperRootContext(PopperRoot popper)
    {
        _popper = popper;
    }
    public ElementReference Anchor { get; set; }
    public EventCallback<ElementReference> OnAnchorChanged { get; set; }
}
