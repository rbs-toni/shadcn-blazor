using System;
using System.Linq;

namespace ShadcnBlazor;

public static class ResizeDirectionExtensions
{
    public static string ToStringFast(this ResizeDirection direction)
    {
        return direction switch
        {
            ResizeDirection.Horizontal => "horizontal",
            ResizeDirection.Vertical => "vertical",
            _ => string.Empty,
        };
    }
}
