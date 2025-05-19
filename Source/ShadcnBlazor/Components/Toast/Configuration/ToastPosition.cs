using System.ComponentModel;

namespace ShadcnBlazor;
public enum ToastPosition
{
    [Description("top-left")]
    TopLeft,

    [Description("top-right")]
    TopRight,

    [Description("top-center")]
    TopCenter,

    [Description("bottom-left")]
    BottomLeft,

    [Description("bottom-right")]
    BottomRight,

    [Description("bottom-center")]
    BottomCenter
}
