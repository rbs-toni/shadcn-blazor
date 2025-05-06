using System;
using System.Linq;

namespace ShadcnBlazor;
public enum AlertVariant
{
    Default,
    Destructive
}

public static class AlertVariantExtensions
{
    public static string ToTailwindClass(this AlertVariant alertVariant)
    {
        return alertVariant switch
        {
            AlertVariant.Default => "bg-background text-foreground",
            AlertVariant.Destructive => "border-destructive/50 text-destructive dark:border-destructive [&>svg]:text-destructive",
            _ => string.Empty,
        };
    }
}
