using System;
using System.Linq;

namespace ShadcnBlazor;
public record FloatingOptions
{
    public FloatingPlacement Placement { get; set; }
    public FloatingStrategy Strategy { get; set; }
    public FloatingOffset? Offset { get; set; }
}
