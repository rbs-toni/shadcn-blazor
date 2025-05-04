using Microsoft.AspNetCore.Components;
using System;
using System.Linq;

namespace ShadcnBlazor;
[EventHandler("ontransitionend", typeof(EventArgs), enableStopPropagation: true, enablePreventDefault: false)]
[EventHandler("onanimationend", typeof(EventArgs), enableStopPropagation: true, enablePreventDefault: false)]
public static class EventHandlers
{
}
