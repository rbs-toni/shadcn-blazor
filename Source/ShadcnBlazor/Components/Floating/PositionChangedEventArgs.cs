using System;
using System.ComponentModel;
using System.Linq;

namespace ShadcnBlazor;
public class PositionChangedEventArgs : EventArgs
{
    public double X { get; set; }
    public double Y { get; set; }
    public string? Placement { get; set; }
    public string? Strategy { get; set; }
}
