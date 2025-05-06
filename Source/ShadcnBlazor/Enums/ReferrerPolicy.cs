using System;
using System.ComponentModel;
using System.Linq;

namespace ShadcnBlazor;
public enum ReferrerPolicy
{
    [Description("no-referrer")]
    NoReferrer,

    [Description("no-referrer-when-downgrade")]
    NoReferrerWhenDowngrade,

    [Description("origin")]
    Origin,

    [Description("origin-when-cross-origin")]
    OriginWhenCrossOrigin,

    [Description("unsafe-url")]
    UnsafeUrl,

    [Description("same-origin")]
    SameOrigin,

    [Description("strict-origin")]
    StrictOrigin,

    [Description("strict-origin-when-cross-origin")]
    StrictOriginWhenCrossOrigin
}
