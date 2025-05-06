using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ShadcnBlazor;
[JsonConverter(typeof(JsonEnumMemberConverter<FloatingPlacement>))]
public enum FloatingPlacement
{
    [EnumMember(Value = "top")]
    Top,

    [EnumMember(Value = "top-start")]
    TopStart,

    [EnumMember(Value = "top-end")]
    TopEnd,

    [EnumMember(Value = "right")]
    Right,

    [EnumMember(Value = "right-start")]
    RightStart,

    [EnumMember(Value = "right-end")]
    RightEnd,

    [EnumMember(Value = "bottom")]
    Bottom,

    [EnumMember(Value = "bottom-start")]
    BottomStart,

    [EnumMember(Value = "bottom-end")]
    BottomEnd,

    [EnumMember(Value = "left")]
    Left,

    [EnumMember(Value = "left-start")]
    LeftStart,

    [EnumMember(Value = "left-end")]
    LeftEnd
}
