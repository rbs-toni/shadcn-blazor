namespace ShadcnBlazor;
// Window-related extensions (not tied to ElementReference)
public static class WindowExtensions
{
    public static async Task CopyToClipboardAsync(this IElementService service, string text)
    { await service.CopyToClipboardAsync(text); }
    public static async Task<double> GetDevicePixelRatioAsync(this IElementService service)
    { return await service.GetDevicePixelRatioAsync(); }
    public static async Task<double> GetWindowInnerHeightAsync(this IElementService service)
    { return await service.GetWindowInnerHeightAsync(); }
    public static async Task<double> GetWindowInnerWidthAsync(this IElementService service)
    { return await service.GetWindowInnerWidthAsync(); }
    public static async Task<string> ReadFromClipboardAsync(this IElementService service)
    { return await service.ReadFromClipboardAsync(); }
}
