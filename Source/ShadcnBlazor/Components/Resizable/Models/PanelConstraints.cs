using System;
using System.Linq;

namespace ShadcnBlazor;

public class PanelConstraints
{
    public double? CollapsedSize { get; set; }
    public bool Collapsible { get; set; }
    public double? DefaultSize { get; set; }
    public double? MaxSize { get; set; }
    public double? MinSize { get; set; }
}
