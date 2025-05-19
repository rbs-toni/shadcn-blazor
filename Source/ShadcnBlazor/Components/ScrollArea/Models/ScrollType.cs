using System;
using System.ComponentModel;
using System.Linq;

namespace ShadcnBlazor;
public enum ScrollType
{
    [Description("auto")]
    Auto,
    [Description("always")]
    Always,
    [Description("scroll")]
    Scroll,
    [Description("hover")]
    Hover
}
