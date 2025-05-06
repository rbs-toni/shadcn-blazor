using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ShadcnBlazor;
[JsonConverter(typeof(JsonEnumMemberConverter<FloatingStrategy>))]
public enum FloatingStrategy
{
    [EnumMember(Value = "fixed")]
    Fixed,
    [EnumMember(Value = "absolute")]
    Absolute
}
