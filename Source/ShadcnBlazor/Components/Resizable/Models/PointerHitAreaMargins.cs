using System;
using System.Linq;

namespace ShadcnBlazor;
public record PointerHitAreaMargins
{
    public PointerHitAreaMargins(int coarse, int fine)
    {
        Coarse = coarse;
        Fine = fine;
    }
    // Coarse inputs (e.g. finger/touch)
    public double Coarse { get; set; } = 15;
    // Fine inputs (e.g. mouse)
    public double Fine { get; set; } = 5;
}
