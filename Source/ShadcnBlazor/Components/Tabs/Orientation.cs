using System;
using System.Linq;

namespace ShadcnBlazor;
public enum Orientation
{
    Horizontal,
    Vertical,
}

public static class OrientationExtensions
{
    public static string ToStringFast(this Orientation orientation)
    {
        return orientation switch
        {
            Orientation.Horizontal => "horizontal",
            Orientation.Vertical => "vertical",
            _ => "",
        };
    }
}
