using System;
using System.Linq;

namespace ShadcnBlazor;

/// <summary>
/// Represents the current state of a drag operation on a splitter handle.
/// </summary>
public class DragState
{
    /// <summary>
    /// The ID of the handle currently being dragged.
    /// </summary>
    public string? DragHandleId { get; set; }

    /// <summary>
    /// The bounding rectangle of the drag handle.
    /// </summary>
    public DomRect? DragHandleRect { get; set; }

    /// <summary>
    /// The initial position of the cursor (either X or Y based on direction) when the drag started.
    /// </summary>
    public double InitialCursorPosition { get; set; }

    /// <summary>
    /// The layout sizes of all panels when the drag began.
    /// </summary>
    public double[] InitialLayout { get; set; } = [];
}
