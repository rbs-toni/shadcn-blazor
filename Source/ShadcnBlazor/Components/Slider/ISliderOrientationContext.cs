using System;
using System.Linq;
using System.Numerics;

namespace ShadcnBlazor;

public interface ISliderOrientationContext
{
    int Direction { get; }
    SlideSide EndEdge { get; }
    string? Size { get; }
    SlideSide StartEdge { get; }
}
