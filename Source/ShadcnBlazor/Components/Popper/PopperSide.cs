using System;
using System.ComponentModel;
using System.Linq;

namespace ShadcnBlazor;
public enum PopperSide
{
    [Description("top")]
    Top,
    [Description("right")]
    Right,
    [Description("bottom")]
    Bottom,
    [Description("left")]
    Left
}

public enum PopperAlign
{
    [Description("start")]
    Start,
    [Description("center")]
    Center,
    [Description("end")]
    End
}

public enum PopperSticky
{
    Partial,
    Always
}

public enum PopperStrategy
{
    Absolute,
    Fixed
}public enum PopperUpdateStrategy
{
    Optimized,
    Always
}
