using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ShadcnBlazor.Docs;
public partial class DocsCode : IAsyncDisposable
{
    const string JSFile = "./_content/ShadcnBlazor.Docs/Components/DocsCode.razor.js";
    string? _codeContent;
    bool _hasCode;
    IJSObjectReference? _jsModule;

    string AssetUrl => $"./_content/ShadcnBlazor.Docs/sources/{Path}.txt";
    [Inject]
    HttpClient HttpClient { get; set; } = default!;
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
            if (_jsModule != null)
            {
                await _jsModule.DisposeAsync();
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
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JSFile);
            if (!string.IsNullOrWhiteSpace(Path))
            {
                await GetCodeContentsAsync();
                await HighlightCodeAsync();
                _hasCode = true;
                StateHasChanged();
            }
        }
    }
    async Task GetCodeContentsAsync()
    {
        HttpClient.BaseAddress ??= new Uri(NavigationManager.BaseUri);

        try
        {
            _codeContent = await StaticAssetService.GetAsync(AssetUrl);
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
    async Task HighlightCodeAsync()
    {
        if (_jsModule != null)
        {
            await _jsModule.InvokeVoidAsync("highlightCode", Ref);
        }
    }
}
