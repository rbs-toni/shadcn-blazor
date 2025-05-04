using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ShadcnBlazor.Docs;
public partial class DocsCode : IAsyncDisposable
{
    const string JSFile = "./_content/ShadcnBlazor.Docs/Components/DocsCode.razor.js";
    string? _codeContent;
    ElementReference _codeRef;
    ElementReference _copyRef;
    bool _hasCode;

    [Inject]
    HttpClient HttpClient { get; set; } = default!;
    IJSObjectReference? JSModule { get; set; }
    [Inject]
    IJSRuntime JSRuntime { get; set; } = default!;
    [Inject]
    NavigationManager NavigationManager { get; set; } = default!;
    [Inject]
    IStaticAssetService StaticAssetService { get; set; } = default!;

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (JSModule != null)
            {
                await JSModule.DisposeAsync();
            }
        }
        catch (Exception)
        {
        }
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            JSModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JSFile);
            if (!string.IsNullOrWhiteSpace(Path))
            {
                await GetCodeContentsAsync();
                _hasCode = true;
                StateHasChanged();
            }
            await JSModule.InvokeVoidAsync("highlightElement", _codeRef);
            await JSModule.InvokeVoidAsync("addCopyButton", _copyRef);
        }
    }
    async Task GetCodeContentsAsync()
    {
        HttpClient.BaseAddress ??= new Uri(NavigationManager.BaseUri);

        try
        {
            _codeContent = await StaticAssetService.GetAsync($"./_content/ShadcnBlazor.Docs/sources/{Path}.txt");
            if (OnLoad.HasDelegate)
            {
                await OnLoad.InvokeAsync(Path);
            }
        }
        catch
        {
            //Do Nothing
        }
    }
}
