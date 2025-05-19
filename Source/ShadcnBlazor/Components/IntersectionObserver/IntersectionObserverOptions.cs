using Microsoft.AspNetCore.Components;
using System;
using System.Linq;

namespace ShadcnBlazor;
public class IntersectionObserverOptions
{
    public ElementReference? Root { get; set; }
    public string RootMargin { get; set; } = "0px";
    public IList<double> Threshold { get; set; } = new List<double> { 1.0 };
}
