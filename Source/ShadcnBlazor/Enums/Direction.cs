using System;
using System.ComponentModel;
using System.Linq;

namespace ShadcnBlazor;
public enum Direction
{
    [Description("ltr")]
    LTR,
    [Description("rtl")]
    RTL,
}

public enum DocumentTheme
{
    [Description("light")]
    Light,
    [Description("dark")]
    Dark,
    [Description("auto")]
    Auto,
}
