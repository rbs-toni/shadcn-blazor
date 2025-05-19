namespace ShadcnBlazor;

public static class ButtonVariants
{
    public static string GetClass(
        StyleType styleType = StyleType.Default,
        ButtonVariant variant = ButtonVariant.Default,
        ButtonSize size = ButtonSize.Default)
    {
        return styleType switch
        {
            StyleType.NewYork => NewYorkStyle(variant, size),
            _ => DefaultStyle(variant, size)
        };
    }

    static string DefaultStyle(ButtonVariant variant, ButtonSize size)
    {
        const string baseClasses =
            "inline-flex items-center justify-center gap-2 whitespace-nowrap rounded-md text-sm font-medium ring-offset-background transition-colors focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50 [&_svg]:pointer-events-none [&_svg]:size-4 [&_svg]:shrink-0";

        var variantClasses = new Dictionary<ButtonVariant, string>
        {
            [ButtonVariant.Default] = "bg-primary text-primary-foreground hover:bg-primary/90",
            [ButtonVariant.Destructive] = "bg-destructive text-destructive-foreground hover:bg-destructive/90",
            [ButtonVariant.Outline] = "border border-input bg-background hover:bg-accent hover:text-accent-foreground",
            [ButtonVariant.Secondary] = "bg-secondary text-secondary-foreground hover:bg-secondary/80",
            [ButtonVariant.Ghost] = "hover:bg-accent hover:text-accent-foreground",
            [ButtonVariant.Link] = "text-primary underline-offset-4 hover:underline"
        };

        var sizeClasses = new Dictionary<ButtonSize, string>
        {
            [ButtonSize.Default] = "h-10 px-4 py-2",
            [ButtonSize.Sm] = "h-9 rounded-md px-3",
            [ButtonSize.Lg] = "h-11 rounded-md px-8",
            [ButtonSize.Icon] = "h-10 w-10"
        };

        return Compose(baseClasses, variantClasses, sizeClasses, variant, size);
    }

    static string NewYorkStyle(ButtonVariant variant, ButtonSize size)
    {
        const string baseClasses =
            "inline-flex items-center justify-center gap-2 whitespace-nowrap rounded-md text-sm font-medium transition-colors focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring disabled:pointer-events-none disabled:opacity-50 [&_svg]:pointer-events-none [&_svg]:size-4 [&_svg]:shrink-0";

        var variantClasses = new Dictionary<ButtonVariant, string>
        {
            [ButtonVariant.Default] = "bg-primary text-primary-foreground shadow hover:bg-primary/90",
            [ButtonVariant.Destructive] = "bg-destructive text-destructive-foreground shadow-sm hover:bg-destructive/90",
            [ButtonVariant.Outline] = "border border-input bg-background shadow-sm hover:bg-accent hover:text-accent-foreground",
            [ButtonVariant.Secondary] = "bg-secondary text-secondary-foreground shadow-sm hover:bg-secondary/80",
            [ButtonVariant.Ghost] = "hover:bg-accent hover:text-accent-foreground",
            [ButtonVariant.Link] = "text-primary underline-offset-4 hover:underline"
        };

        var sizeClasses = new Dictionary<ButtonSize, string>
        {
            [ButtonSize.Default] = "h-9 px-4 py-2",
            [ButtonSize.Xs] = "h-7 rounded px-2",
            [ButtonSize.Sm] = "h-8 rounded-md px-3 text-xs",
            [ButtonSize.Lg] = "h-10 rounded-md px-8",
            [ButtonSize.Icon] = "h-9 w-9"
        };

        return Compose(baseClasses, variantClasses, sizeClasses, variant, size);
    }

    static string Compose(
        string baseClasses,
        Dictionary<ButtonVariant, string> variantMap,
        Dictionary<ButtonSize, string> sizeMap,
        ButtonVariant variant,
        ButtonSize size)
    {
        var variantClass = variantMap.TryGetValue(variant, out var v) ? v : variantMap[ButtonVariant.Default];
        var sizeClass = sizeMap.TryGetValue(size, out var s) ? s : sizeMap[ButtonSize.Default];

        return $"{baseClasses} {variantClass} {sizeClass}";
    }
}


