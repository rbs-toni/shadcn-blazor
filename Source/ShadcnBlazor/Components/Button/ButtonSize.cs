using System.ComponentModel;

namespace ShadcnBlazor;

/// <summary>
/// Specifies the size of a button.
/// </summary>
public enum ButtonSize
{
    Default,
    Xs,
    Sm,
    Lg,
    Icon
}

public static class ButtonSizeExtensions
{
    static readonly IReadOnlyDictionary<StyleType, IReadOnlyDictionary<ButtonSize, string>> ButtonSizeClasses =
        new Dictionary<StyleType, IReadOnlyDictionary<ButtonSize, string>>
    {
        [StyleType.Default] =
            new Dictionary<ButtonSize, string>
            {
                [ButtonSize.Default] = "h-10 px-4 py-2",
                [ButtonSize.Sm] = "h-9 rounded-md px-3",
                [ButtonSize.Lg] = "h-11 rounded-md px-8",
                [ButtonSize.Icon] = "h-10 w-10"
            },
        [StyleType.NewYork] =
            new Dictionary<ButtonSize, string>
            {
                [ButtonSize.Default] = "h-9 px-4 py-2",
                [ButtonSize.Xs] = "h-7 rounded px-2",
                [ButtonSize.Sm] = "h-8 rounded-md px-3 text-xs",
                [ButtonSize.Lg] = "h-10 rounded-md px-8",
                [ButtonSize.Icon] = "h-9 w-9"
            }
    };

    public static string? GetTailwindClass(this ButtonSize buttonSize, StyleType style)
    {
        // First, try exact match
        if (ButtonSizeClasses.TryGetValue(style, out var styleMap))
        {
            if (styleMap.TryGetValue(buttonSize, out var exactMatch))
                return exactMatch;

            // Fallback to ButtonSize.Default within the same style
            if (styleMap.TryGetValue(ButtonSize.Default, out var fallback))
                return fallback;
        }

        // Finally, try ButtonSize.Default from StyleType.Default
        if (ButtonSizeClasses.TryGetValue(StyleType.Default, out var defaultMap) &&
            defaultMap.TryGetValue(ButtonSize.Default, out var finalFallback))
        {
            return finalFallback;
        }

        // If absolutely nothing found
        return default;
    }
}

