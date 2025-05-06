namespace ShadcnBlazor;
public enum ButtonVariant
{
    Default,
    Destructive,
    Outline,
    Secondary,
    Ghost,
    Link
}

public static class ButtonVariantExtensions
{
    static readonly IReadOnlyDictionary<StyleType, IReadOnlyDictionary<ButtonVariant, string>> ButtonVariantClasses =
        new Dictionary<StyleType, IReadOnlyDictionary<ButtonVariant, string>>
        {
            [StyleType.Default] =
            new Dictionary<ButtonVariant, string>
            {
                [ButtonVariant.Default] = "bg-primary text-primary-foreground hover:bg-primary/90",
                [ButtonVariant.Destructive] = "bg-destructive text-destructive-foreground hover:bg-destructive/90",
                [ButtonVariant.Outline] = "border border-input bg-background hover:bg-accent hover:text-accent-foreground",
                [ButtonVariant.Secondary] = "bg-secondary text-secondary-foreground hover:bg-secondary/80",
                [ButtonVariant.Ghost] = "hover:bg-accent hover:text-accent-foreground",
                [ButtonVariant.Link] = "text-primary underline-offset-4 hover:underline",
            },
            [StyleType.NewYork] =
            new Dictionary<ButtonVariant, string>
            {
                [ButtonVariant.Default] = "bg-primary text-primary-foreground shadow hover:bg-primary/90",
                [ButtonVariant.Destructive] = "bg-destructive text-destructive-foreground shadow-sm hover:bg-destructive/90",
                [ButtonVariant.Outline] = "border border-input bg-background shadow-sm hover:bg-accent hover:text-accent-foreground",
                [ButtonVariant.Secondary] = "bg-secondary text-secondary-foreground shadow-sm hover:bg-secondary/80",
                [ButtonVariant.Ghost] = "hover:bg-accent hover:text-accent-foreground",
                [ButtonVariant.Link] = "text-primary underline-offset-4 hover:underline",
            }
        };
    public static string? GetTailwindClass(this ButtonVariant variant, StyleType style)
    {
        // First, try exact match
        if (ButtonVariantClasses.TryGetValue(style, out var styleMap))
        {
            if (styleMap.TryGetValue(variant, out var exactMatch))
                return exactMatch;

            // Fallback to ButtonSize.Default within the same style
            if (styleMap.TryGetValue(ButtonVariant.Default, out var fallback))
                return fallback;
        }

        // Finally, try ButtonSize.Default from StyleType.Default
        if (ButtonVariantClasses.TryGetValue(StyleType.Default, out var defaultMap) &&
            defaultMap.TryGetValue(ButtonVariant.Default, out var finalFallback))
        {
            return finalFallback;
        }

        // If absolutely nothing found
        return default;
    }
}
