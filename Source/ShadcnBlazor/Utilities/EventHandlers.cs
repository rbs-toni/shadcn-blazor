using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Linq;

namespace ShadcnBlazor;
[EventHandler("ontransitionend", typeof(EventArgs), enableStopPropagation: true, enablePreventDefault: false)]
[EventHandler("onanimationend", typeof(EventArgs), enableStopPropagation: true, enablePreventDefault: false)]
[EventHandler("onpositionchanged", typeof(PositionChangedEventArgs), enableStopPropagation: true, enablePreventDefault: false)]
[EventHandler("onextpointerdown", typeof(ExtendedPointerEventArgs), enableStopPropagation: true, enablePreventDefault: false)]
[EventHandler("onextpointermove", typeof(ExtendedPointerEventArgs), enableStopPropagation: true, enablePreventDefault: false)]
[EventHandler("onextpointerup", typeof(ExtendedPointerEventArgs), enableStopPropagation: true, enablePreventDefault: false)]
[EventHandler("onextfocus", typeof(ExtendedFocusEventArgs), enableStopPropagation: true, enablePreventDefault: false)]
[EventHandler("onextblur", typeof(ExtendedFocusEventArgs), enableStopPropagation: true, enablePreventDefault: false)]
public static class EventHandlers
{
}

public class ExtendedPointerEventArgs : PointerEventArgs
{
    /// <summary>
    /// Gets or sets the ID of the target element that triggered the event.
    /// This assumes the target element has an Id attribute.
    /// </summary>
    public string? Target { get; set; }

    /// <summary>
    /// Gets or sets the ID of the related target element (for events like mouseenter/mouseleave).
    /// This assumes the related target element has an Id attribute.
    /// </summary>
    public string? RelatedTarget { get; set; }

    /// <summary>
    /// Gets or sets the HTML tag name of the target element.
    /// </summary>
    public string? TagName { get; set; }
}

public class ExtendedFocusEventArgs : FocusEventArgs
{
    public string? Target { get; set; }
    public string? TagName { get; set; }
    public bool IsContained { get; set; }
}

