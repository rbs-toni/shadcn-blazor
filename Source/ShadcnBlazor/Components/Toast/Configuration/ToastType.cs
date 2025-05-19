using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace ShadcnBlazor;
public enum ToastType
{
    [Description("normal")]
    Normal,
    [Description("action")]
    Action,
    [Description("success")]
    Success,
    [Description("info")]
    Info,
    [Description("warning")]
    Warning,
    [Description("error")]
    Error,
    [Description("loading")]
    Loading,
    [Description("default")]
    Default
}
