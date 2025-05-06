using System;
using System.Linq;

namespace ShadcnBlazor;
public static class TabsUtils
{
    public static string MakeTriggerId(string? baseId, string? value)
    {
        ArgumentNullException.ThrowIfNull(baseId, nameof(baseId));
        ArgumentNullException.ThrowIfNull(value, nameof(value));

        return $"{baseId}-trigger-{value}";
    }

    public static string MakeContentId(string? baseId, string? value)
    {
        ArgumentNullException.ThrowIfNull(baseId, nameof(baseId));
        ArgumentNullException.ThrowIfNull(value, nameof(value));

        return $"{baseId}-content-{value}";
    }
}
