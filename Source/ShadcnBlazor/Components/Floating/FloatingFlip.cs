using System;
using System.Linq;

namespace ShadcnBlazor;
public record FloatingFlip
{
    public bool MainAxis { get; set; } = true;
    public bool CrossAxis { get; set; } = true;
    public FloatingFallbackAxisSideDirection FallbackAxisSideDirection { get; set; } = FloatingFallbackAxisSideDirection.None;
    public bool FlipAlignment { get; set; } = true;
    public HashSet<FloatingPlacement>? FallbackPlacements { get; set; }
    public FloatingFallbackStrategy FallbackStrategy { get; set; }
}
