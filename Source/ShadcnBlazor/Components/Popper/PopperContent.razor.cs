using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShadcnBlazor;
public partial class PopperContent : IAsyncDisposable
{
    const string JS_FILE = "./_content/ShadcnBlazor/Components/Popper/PopperContent.razor.js";
    readonly PopperContentContext _context;
    bool _anchorReady;
    IJSObjectReference? _floatingInstance;
    IJSObjectReference? _jsModule;
    bool _popperReady;
    ElementReference _popperRef;

    public PopperContent() { _context = new(this); }

    ElementReference Anchor => Reference ?? RootContext?.Anchor ?? throw new ArgumentNullException(nameof(Anchor));
    bool IsPopperReady => _anchorReady && _popperReady;
    [Inject]
    IJSRuntime JSRuntime { get; set; } = default!;
    string? StyleValue => new StyleBuilder(Style)
        .AddStyle("left", "0")
        .AddStyle("top", "-200%")
        .AddStyle("min-width", "max-content")
        .AddStyle("position", Strategy.ToAttributeValue(false))
        .Build();

    public async ValueTask DisposeAsync()
    {
        if (RootContext != null)
        {
            RootContext.OnAnchorChanged -= HandleAnchorChanged;
        }
        if (_jsModule != null)
        {
            await _jsModule.DisposeAsync();
        }
        if (_floatingInstance != null)
        {
            await _floatingInstance.InvokeVoidAsync("cleanUp");
            await _floatingInstance.DisposeAsync();
        }
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JS_FILE);
        }
        if (_floatingInstance == null && IsPopperReady)
        {
            await CreatePopperAsync();
        }
    }
    protected override void OnInitialized()
    {
        if (RootContext != null)
        {
            RootContext.OnAnchorChanged += HandleAnchorChanged;
        }
        base.OnInitialized();
    }
    PopperOptions BuildPopperOptions()
    {
        return new PopperOptions()
        {
            Placement = DesiredPlacement,
            AlignOffset = AlignOffset,
            SideOffset = SideOffset,
            Strategy = Strategy.ToAttributeValue(false),
            Arrow = _context.Arrow
        };
    }
    async Task CheckPopperReadiness()
    {
        if (_popperReady && _anchorReady)
        {
            await CreatePopperAsync();
        }
    }
    async Task CreatePopperAsync()
    {
        if (_jsModule != null)
        {
            _floatingInstance = await _jsModule.InvokeAsync<IJSObjectReference>(
                "create",
                Anchor,
                _popperRef,
                BuildPopperOptions(),
                AutoUpdateOptions);
            if (_floatingInstance != null)
            {
                OnPlaced?.InvokeAsync();
            }
        }
    }
    void HandleAnchorChanged()
    {
        _anchorReady = true;
        InvokeAsync(CheckPopperReadiness);
    }
    async Task HandlePopperRefChanged(ElementReference reference)
    {
        _popperReady = true;
        await CheckPopperReadiness();
    }
}
