using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadcnBlazor;
public class CarouselOptions
{
    /// <summary>
    /// Setting this to false will not activate or deactivate the carousel. Useful for breakpoints.
    /// </summary>
    public bool Active { get; set; } = true;

    /// <summary>
    /// Align the slides relative to the carousel viewport. Use "start", "center", or "end".
    /// </summary>
    public string? Align { get; set; } = "center";

    /// <summary>
    /// Choose scroll axis between "x" and "y".
    /// </summary>
    public string Axis { get; set; } = "x";

    /// <summary>
    /// An object with options applied for breakpoints via media queries.
    /// </summary>
    public Dictionary<string, CarouselOptions>? Breakpoints { get; set; }

    /// <summary>
    /// A custom container element which holds the slides.
    /// </summary>
    public string? Container { get; set; } // string | HTMLElement | null

    /// <summary>
    /// Clear empty space. Use "trimSnaps", "keepSnaps", or false.
    /// </summary>
    public string? ContainScroll { get; set; } = "trimSnaps"; // string | false

    /// <summary>
    /// Choose content direction: "ltr" or "rtl".
    /// </summary>
    public string Direction { get; set; } = "ltr";

    /// <summary>
    /// Enables momentum scrolling.
    /// </summary>
    public bool DragFree { get; set; }

    /// <summary>
    /// Drag threshold in pixels (only for mouse events).
    /// </summary>
    public int DragThreshold { get; set; } = 10;

    /// <summary>
    /// Scroll duration (not milliseconds). Recommended: 20–60.
    /// </summary>
    public int Duration { get; set; } = 25;

    /// <summary>
    /// This is the Intersection Observer threshold option that will be applied to all slides.
    /// </summary>
    public double InViewThreshold { get; set; } = 0;

    /// <summary>
    /// Enables infinite looping. Embla will apply translateX or translateY to the slides that need to change position in order to create the loop effect.
    /// </summary>
    public bool Loop { get; set; }

    /// <summary>
    /// Allow the carousel to skip scroll snaps if it's dragged vigorously. Note that this option will be ignored if the dragFree option is set to true.
    /// </summary>
    public bool SkipSnaps { get; set; }

    /// <summary>
    /// Enables using custom slide elements. By default, Embla will choose all direct child elements of its container. Provide either a valid CSS selector string or a nodeList/array containing HTML elements.
    /// </summary>
    public string? Slides { get; set; }  // string | HTMLElement[] | NodeList | null

    /// <summary>
    /// Group slides together. Drag interactions, dot navigation, and previous/next buttons are mapped to group slides into the given number, which has to be an integer. Set it to auto if you want Embla to group slides automatically.
    /// </summary>
    public string SlidesToScroll { get; set; } = "1";

    /// <summary>
    /// Set the initial scroll snap to the given number. First snap index starts at 0. Please note that this is not necessarily equal to the number of slides when used together with the slidesToScroll option.
    /// </summary>
    public int StartIndex { get; set; }

    /// <summary>
    /// Enables for scrolling the carousel with mouse and touch interactions. Set this to false to disable drag events or pass a custom callback to add your own drag logic.
    /// </summary>
    public bool WatchDrag { get; set; } = true;

    /// <summary>
    /// Embla automatically watches the slides for focus events. The default callback fires the slideFocus event and scrolls to the focused element. Set this to false to disable this behaviour or pass a custom callback to add your own focus logic.
    /// </summary>
    public bool WatchFocus { get; set; } = true;

    /// <summary>
    /// Embla automatically watches the container and slides for size changes and runs reInit when any size has changed. Set this to false to disable this behaviour or pass a custom callback to add your own resize logic.
    /// </summary>
    public bool WatchResize { get; set; } = true;

    /// <summary>
    /// Embla automatically watches the container for added and/or removed slides and runs reInit if needed. Set this to false to disable this behaviour or pass a custom callback to add your own slides changed logic.
    /// </summary>
    public bool WatchSlides { get; set; } = true;
}
