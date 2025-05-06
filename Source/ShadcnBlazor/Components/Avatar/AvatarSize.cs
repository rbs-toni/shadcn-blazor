using System;
using System.ComponentModel;
using System.Linq;

namespace ShadcnBlazor;
public enum AvatarSize
{
    [Description("h-10 w-10 text-xs")]
    Small,
    [Description("h-16 w-16 text-2xl")]
    Base,
    [Description("h-32 w-32 text-5xl")]
    Large,
}

public enum ImageLoadingStatus
{
    Idle,
    Loading,
    Loaded,
    Error
}
