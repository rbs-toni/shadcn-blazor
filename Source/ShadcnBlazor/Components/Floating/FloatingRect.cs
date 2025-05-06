using System;
using System.Linq;

namespace ShadcnBlazor;
public record FloatingRect
{
    /// <summary>
    /// The x-coordinate of the floating element.
    /// </summary>
    public float X { get; set; }
    /// <summary>
    /// The y-coordinate of the floating element.
    /// </summary>
    public float Y { get; set; }
    public FloatingPlacement Placement { get; set; }
    public FloatingStrategy Strategy { get; set; }
}
