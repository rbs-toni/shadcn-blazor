namespace ShadcnBlazor;

public readonly struct StyleBuilder
{
    readonly HashSet<string> _styles;
    readonly string? _userStyles;

    /// <summary>
    /// Initializes a new instance of the <see cref="StyleBuilder"/> class.
    /// </summary>
    public StyleBuilder()
    {
        _styles = [];
        _userStyles = null;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StyleBuilder"/> class.
    /// </summary>
    /// <param name="userStyles">The user styles to include at the end.</param>
    public StyleBuilder(string? userStyles)
    {
        _styles = [];
        _userStyles = string.IsNullOrWhiteSpace(userStyles)
                    ? null
                    : string.Join("; ", userStyles.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                                                  .Where(i => !string.IsNullOrWhiteSpace(i)));
    }

    /// <summary>
    /// Adds a conditional in-line style to the builder with space separator and closing semicolon.
    /// </summary>
    /// <param name="style"></param>
    public StyleBuilder AddStyle(string? style) => AddRaw(style);

    /// <summary>
    /// Adds a conditional in-line style to the builder with space separator and closing semicolon..
    /// </summary>
    /// <param name="prop"></param>
    /// <param name="value">Style to add</param>
    /// <returns>StyleBuilder</returns>
    public StyleBuilder AddStyle(string prop, string? value) => AddRaw($"{prop}: {value}");

    /// <summary>
    /// Adds a conditional in-line style to the builder with space separator and closing semicolon..
    /// </summary>
    /// <param name="prop"></param>
    /// <param name="value">Style to conditionally add.</param>
    /// <param name="when">Condition in which the style is added.</param>
    /// <returns>StyleBuilder</returns>
    public StyleBuilder AddStyle(string prop, string? value, bool when = true) => when ? AddStyle(prop, value) : this;

    /// <summary>
    /// Adds a conditional in-line style to the builder with space separator and closing semicolon..
    /// </summary>
    /// <param name="prop"></param>
    /// <param name="value">Style to conditionally add.</param>
    /// <param name="when">Condition in which the style is added.</param>
    /// <returns>StyleBuilder</returns>
    public StyleBuilder AddStyle(string prop, string? value, Func<bool> when) => AddStyle(prop, value, when != null && when());

    /// <summary>
    /// Finalize the completed Style as a string.
    /// </summary>
    /// <returns>string</returns>
    public string? Build()
    {
        var allStyles = string.IsNullOrWhiteSpace(_userStyles)
                      ? _styles
                      : _styles.Union(new[] { _userStyles });

        if (!allStyles.Any())
        {
            return null;
        }

        return string.Concat(allStyles.Select(s => $"{s}; ")).TrimEnd();
    }

    /// <summary>
    /// ToString should only and always call Build to finalize the rendered string.
    /// </summary>
    /// <returns></returns>
    public override string? ToString() => Build();

    /// <summary>
    /// Adds a raw string to the builder that will be concatenated with the next style or value added to the builder.
    /// </summary>
    /// <param name="style"></param>
    /// <returns>StyleBuilder</returns>
    StyleBuilder AddRaw(string? style)
    {
        if (!string.IsNullOrWhiteSpace(style))
        {
            _styles.Add(style.Trim().TrimEnd(';'));
        }

        return this;
    }
}

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

    private static string DefaultStyle(ButtonVariant variant, ButtonSize size)
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

    private static string NewYorkStyle(ButtonVariant variant, ButtonSize size)
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

    private static string Compose(
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

