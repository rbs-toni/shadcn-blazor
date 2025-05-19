using System;
using System.Linq;

namespace ShadcnBlazor;
public class IntersectionObserverEntry
{
    public bool IsIntersecting { get; set; }
    public double IntersectionRatio { get; set; }
    public DomRect? BoundingClientRect { get; set; }
    public DomRect? RootBounds { get; set; }
    public bool IsVisible { get; set; }
    public double Time { get; set; }
}
