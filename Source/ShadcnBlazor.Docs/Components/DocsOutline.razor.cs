using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using System;
using System.Linq;

namespace ShadcnBlazor.Docs;
public partial class DocsOutline : IAsyncDisposable
{
    const string JSFile = "./_content/ShadcnBlazor.Docs/Components/DocsOutline.razor.js";
    List<Header>? _anchors;
    bool _isDisposed;
    IJSObjectReference? _jsModule;

    [Inject]
    IJSRuntime JSRuntime { get; set; } = default!;

    [Inject]
    NavigationManager NavigationManager { get; set; } = default!;

    [CascadingParameter]
    LayoutContext LayoutContext { get; set; } = default!;

    public async ValueTask DisposeAsync()
    {
        await DisposeCoreAsync().ConfigureAwait(false);
        GC.SuppressFinalize(this);
    }
    protected virtual async ValueTask DisposeCoreAsync()
    {
        if (_isDisposed)
        {
            return;
        }

        if (_jsModule != null)
        {
            try
            {
                NavigationManager.LocationChanged -= LocationChanged;
                await _jsModule.DisposeAsync().ConfigureAwait(false);
            }
            catch (InvalidOperationException)
            {
                // This can be called too early when using prerendering
            }
            catch (Exception ex) when (ex is JSDisconnectedException ||
                                       ex is OperationCanceledException)
            {
                // The JSRuntime side may routinely be gone already if the reason we're disposing is that
                // the client disconnected. This is not an error.
            }
        }
        _isDisposed = true;
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>(
                 "import",
                 JSFile);
            if (_jsModule != null)
            {
                await GetHeadersAsync();
            }
        }
    }
    protected override void OnInitialized()
    {
        NavigationManager.LocationChanged += LocationChanged;
    }
    async void LocationChanged(object? sender, LocationChangedEventArgs e)
    {
        try
        {
            await GetHeadersAsync();
        }
        catch (Exception)
        {
            // Already disposed
        }
    }
    async Task GetHeadersAsync()
    {
        if (_jsModule != null)
        {
            List<Header>? anchors = await _jsModule.InvokeAsync<List<Header>?>("getHeaders");
            if (HeadersEqual(_anchors, anchors))
            {
                return;
            }
            _anchors = anchors;
            if (_anchors != null)
            {
                LayoutContext.SetHeaders(_anchors);
            }
            StateHasChanged();
        }
    }
    bool HeadersEqual(List<Header>? firstSet, List<Header>? secondSet)
    {
        return (firstSet ?? [])
            .SequenceEqual(secondSet ?? []);
    }
}
