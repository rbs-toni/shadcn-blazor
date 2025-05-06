using System;
using System.Linq;

namespace ShadcnBlazor;

/// <summary>
/// Shifts the floating element to keep it in view.
/// </summary>
public record FloatingShift
{
    /// <summary>
    /// This is the main axis in which shifting is applied.
    /// </summary>
    public bool MainAxis { get; set; } = true;

    /// <summary>
    /// This is the cross axis in which shifting is applied, the opposite axis of <see cref="MainAxis">MainAxis</see>.
    /// </summary>
    public bool CrossAxis { get; set; }
}
