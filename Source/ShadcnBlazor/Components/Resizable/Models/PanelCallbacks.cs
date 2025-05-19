using Microsoft.AspNetCore.Components;
using System;
using System.Linq;

namespace ShadcnBlazor;
public class PanelCallbacks
{
    public EventCallback OnCollapse { get; set; }
    public EventCallback OnExpand { get; set; }
    public EventCallback<PanelResizeEventArgs> OnResize { get; set; }
}
