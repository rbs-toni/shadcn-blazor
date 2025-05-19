using System;
using System.Linq;

namespace ShadcnBlazor;
public record ScrollSize
{
    public double Content { get; set; }
    public double ViewPort { get; set; }
    public Scrollbar? Scrollbar { get; set; }
}
