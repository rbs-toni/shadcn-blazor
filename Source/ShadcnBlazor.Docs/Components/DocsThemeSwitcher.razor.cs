using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Linq;

namespace ShadcnBlazor.Docs;
public partial class DocsThemeSwitcher:IAsyncDisposable
{
    [Inject]
    IJSRuntime JSRuntime { get; set; } = default!;

    const string JSFile = "./_content/ShadcnBlazor.Docs/Components/DocsThemeSwitcher.razor.js";
    IJSObjectReference? _module;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _module = await JSRuntime.InvokeAsync<IJSObjectReference>(
                 "import",
                 JSFile);
            await _module.InvokeVoidAsync("init");
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_module!=null)
        {
            await _module.DisposeAsync();
        }
    }

    public async Task ToggleThemeAsync()
    {
        if (_module != null)
        {
            await _module.InvokeVoidAsync("toggleTheme");
        }
    }
}
