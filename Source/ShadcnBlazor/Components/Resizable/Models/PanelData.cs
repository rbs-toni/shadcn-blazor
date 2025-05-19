using System;
using System.Linq;

namespace ShadcnBlazor;

public class PanelData
{
    public string? Id { get; set; }
    public int? Order { get; set; }
    public PanelCallbacks Callbacks { get; set; } = new();
    public PanelConstraints Constraints { get; set; } = new();
    public bool IdIsFromProps { get; set; }
}

public class PanelResizeEventArgs : EventArgs
{
    public double Size { get; }
    public double? PreviousSize { get; }

    public PanelResizeEventArgs(double size, double? previousSize)
    {
        Size = size;
        PreviousSize = previousSize;
    }
}
