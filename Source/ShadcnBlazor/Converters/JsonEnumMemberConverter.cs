using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ShadcnBlazor;
public class JsonEnumMemberConverter<T> : JsonConverter<T> where T : Enum
{
    static readonly ConcurrentDictionary<string, T> FromValue = new();
    static readonly ConcurrentDictionary<T, string> ToValue = new();

    static JsonEnumMemberConverter()
    {
        foreach (var field in typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            var attribute = field.GetCustomAttribute<EnumMemberAttribute>();
            var value = (T?)field.GetValue(null);
            var enumValue = attribute?.Value ?? value?.ToString();
            FromValue.TryAdd(enumValue, value);
            ToValue.TryAdd(value, enumValue);
        }
    }

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var enumValue = reader.GetString();
        if (enumValue != null && FromValue.TryGetValue(enumValue, out var value))
        {
            return value;
        }

        throw new JsonException($"Unable to convert \"{enumValue}\" to Enum \"{typeof(T)}\".");
    }
    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        if (ToValue.TryGetValue(value, out var enumValue))
        {
            writer.WriteStringValue(enumValue);
        }
        else
        {
            throw new JsonException($"Unable to convert Enum \"{value}\" to string.");
        }
    }
}

