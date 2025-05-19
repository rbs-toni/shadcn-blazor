using System;
using System.Linq;

namespace ShadcnBlazor;
public class IntersectionObserverContext
{
    public IntersectionObserverEntry Entry { get; set; }
    public bool IsIntersecting => Entry?.IsIntersecting ?? false;
    public ForwardRef Ref { get; set; } = new ForwardRef();
}
