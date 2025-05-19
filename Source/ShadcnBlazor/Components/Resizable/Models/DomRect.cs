using System;
using System.Linq;

namespace ShadcnBlazor;

/// <summary>
/// Represents the size and position of a rectangle in the DOM.
/// Mirrors the structure of the JavaScript DOMRect object.
/// </summary>
public class DomRect
{
    /// <summary>
    /// The height of the rectangle in pixels.
    /// </summary>
    public double Height { get; set; }

    /// <summary>
    /// The width of the rectangle in pixels.
    /// </summary>
    public double Width { get; set; }

    /// <summary>
    /// The x-coordinate (horizontal position) of the rectangle’s origin.
    /// </summary>
    public double X { get; set; }

    /// <summary>
    /// The y-coordinate (vertical position) of the rectangle’s origin.
    /// </summary>
    public double Y { get; set; }
    public double Top { get; set; }
    public double Left { get; set; }
}
