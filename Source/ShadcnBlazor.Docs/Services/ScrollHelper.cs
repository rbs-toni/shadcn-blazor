using Microsoft.JSInterop;

namespace ShadcnBlazor.Docs;
public class ScrollHelper : JSModule, IScrollHelper
{
    private const string JSFile = "./_content/ShadcnBlazor.Docs/js/ScrollHelper.js";
    public ScrollHelper(IJSRuntime jsRuntime) : base(jsRuntime, JSFile)
    {
    }
    public async ValueTask ScrollToFragmentAsync(string elementId)
    {
        await InvokeVoidAsync("scrollToFragment", elementId);
    }
}
