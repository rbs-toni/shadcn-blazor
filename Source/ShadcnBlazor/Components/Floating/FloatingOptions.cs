using System;
using System.Linq;

namespace ShadcnBlazor;
public record FloatingOptions
{
    public FloatingOptions(string placement, FloatingStrategy strategy, FloatingOffset offset)
    {
        Placement = placement;
        Strategy = strategy;
        Offset = offset;
    }
    public string Placement { get; set; }
    public FloatingStrategy Strategy { get; set; }
    public FloatingOffset? Offset { get; set; }
}
