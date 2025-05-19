using System;
using System.Linq;

namespace ShadcnBlazor;
static class ToggleVariants
{
    static readonly Dictionary<StyleType, StyleConfiguration> StyleConfigurations = new()
    {
        {
            StyleType.Default, new StyleConfiguration(
                BaseClasses: "inline-flex items-center justify-center rounded-md text-sm font-medium ring-offset-background transition-colors hover:bg-muted hover:text-muted-foreground focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50 data-[state=on]:bg-accent data-[state=on]:text-accent-foreground [&_svg]:pointer-events-none [&_svg]:size-4 [&_svg]:shrink-0 gap-2",
                VariantClasses: new Dictionary<ToggleVariant, string>
                {
                    { ToggleVariant.Default, "bg-transparent" },
                    { ToggleVariant.Outline, "border border-input bg-transparent hover:bg-accent hover:text-accent-foreground" }
                },
                SizeClasses: new Dictionary<ToggleSize, string>
                {
                    { ToggleSize.Default, "h-10 px-3 min-w-10" },
                    { ToggleSize.Sm, "h-9 px-2.5 min-w-9" },
                    { ToggleSize.Lg, "h-11 px-5 min-w-11" }
                })
        },
        {
            StyleType.NewYork, new StyleConfiguration(
                BaseClasses: "inline-flex items-center justify-center gap-2 rounded-md text-sm font-medium transition-colors hover:bg-muted hover:text-muted-foreground focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring disabled:pointer-events-none disabled:opacity-50 data-[state=on]:bg-accent data-[state=on]:text-accent-foreground [&_svg]:pointer-events-none [&_svg]:size-4 [&_svg]:shrink-0",
                VariantClasses: new Dictionary<ToggleVariant, string>
                {
                    { ToggleVariant.Default, "bg-transparent" },
                    { ToggleVariant.Outline, "border border-input bg-transparent shadow-sm hover:bg-accent hover:text-accent-foreground" }
                },
                SizeClasses: new Dictionary<ToggleSize, string>
                {
                    { ToggleSize.Default, "h-9 px-2 min-w-9" },
                    { ToggleSize.Sm, "h-8 px-1.5 min-w-8" },
                    { ToggleSize.Lg, "h-10 px-2.5 min-w-10" }
                })
        }
    };

    public static string BuildClass(StyleType styleType, ToggleVariant variant, ToggleSize size)
    {
        if (!StyleConfigurations.TryGetValue(styleType, out var config))
        {
            throw new ArgumentException($"Unsupported style type: {styleType}");
        }

        var variantClass = config.VariantClasses.TryGetValue(variant, out var v) ? v : string.Empty;
        var sizeClass = config.SizeClasses.TryGetValue(size, out var s) ? s : string.Empty;

        return $"{config.BaseClasses} {variantClass} {sizeClass}".Trim();
    }

    record StyleConfiguration(
        string BaseClasses,
        Dictionary<ToggleVariant, string> VariantClasses,
        Dictionary<ToggleSize, string> SizeClasses
    );
}
