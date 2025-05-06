namespace ShadcnBlazor;
public enum SwapEffect
{
    Flip,
    Rotate,
}

public static class SwapEffectExtensions
{
    public static string ToStringFast(this SwapEffect swapEffect)
    {
        return swapEffect switch
        {
            SwapEffect.Flip => "swap-flip",
            SwapEffect.Rotate => "swap-rotate",
            _ => ""
        };
    }
}
