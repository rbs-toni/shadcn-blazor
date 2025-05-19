using Microsoft.AspNetCore.Components;

namespace ShadcnBlazor;
public readonly struct CustomComponent
{
    public ComponentParameters? Parameters { get; }
    public RenderFragment Fragment { get; }

    CustomComponent(RenderFragment fragment, ComponentParameters? parameters = null)
    {
        Fragment = fragment;
        Parameters = parameters;
    }
}

