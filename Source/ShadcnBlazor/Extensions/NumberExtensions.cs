namespace ShadcnBlazor;
public static class NumberExtensions
{
    public static string? ToPx(this int value)
    {
        return value + "px";
    }

    public static string? ToPx(this double value)
    {
        return value + "px";
    }
}
