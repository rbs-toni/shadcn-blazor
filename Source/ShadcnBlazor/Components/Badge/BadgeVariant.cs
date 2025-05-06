using System.ComponentModel;

namespace ShadcnBlazor;
public enum BadgeVariant
{
    [Description("border-transparent bg-primary text-primary-foreground shadow hover:bg-primary/80")]
    Default,

    [Description("border-transparent bg-secondary text-secondary-foreground hover:bg-secondary/80")]
    Secondary,

    [Description("border-transparent bg-destructive text-destructive-foreground shadow hover:bg-destructive/80")]
    Destructive,

    [Description("text-foreground")]
    Outline
}
