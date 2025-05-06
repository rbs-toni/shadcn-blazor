using System;
using System.Linq;

namespace ShadcnBlazor;
/// <summary>
/// Translates the floating element along the specified axes.
/// </summary>
public record FloatingOffset
{
    public FloatingOffset(float mainAxis)
    {
        MainAxis = mainAxis;
    }
    public FloatingOffset(float mainAxis, float crossAxis, float? alignmentAxis = null)
    {
        MainAxis = mainAxis;
        CrossAxis = crossAxis;
        AlignmentAxis = alignmentAxis;
    }
    /// <summary>
    /// The axis that runs along the side of the floating element. 
    /// Represents the distance (gutter or margin) between the floating element and the reference element.
    /// </summary>
    public float MainAxis { get; set; }

    /// <summary>
    /// The axis that runs along the alignment of the floating element. 
    /// Represents the skidding between the floating element and the reference element.
    /// </summary>
    public float CrossAxis { get; set; }

    /// <summary>
    /// The same axis as <see cref="CrossAxis">CrossAxis</see> but applies only to aligned placements and inverts the end alignment. When set to a number, it overrides the <see cref="CrossAxis">CrossAxis</see> value.
    /// </summary>
    public float? AlignmentAxis { get; set; }
}
