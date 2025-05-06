using System;
using System.Linq;

namespace ShadcnBlazor;
public enum Direction
{
    LeftToRight,
    RightToLeft,
}

public static class DirectionExtensions
{
    public static string? ToStringFast(this Direction direction)
    {
        return direction switch
        {
            Direction.LeftToRight => "ltr",
            Direction.RightToLeft => "rtl",
            _ => default,
        };
    }
}
