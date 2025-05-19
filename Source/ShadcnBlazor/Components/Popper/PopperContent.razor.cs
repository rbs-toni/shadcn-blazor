using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Linq;

namespace ShadcnBlazor;
public partial class PopperContent : IAsyncDisposable
{
    const string JS_FILE = "./_content/ShadcnBlazor/Components/Popper/PopperContent.razor.js";
    readonly PopperContentContext _context;
    DotNetObjectReference<PopperContent>? _dotNetRef;
    IJSObjectReference? _floatingInstance;
    ElementReference _floatingRef;
    IJSObjectReference? _jsModule;

    public PopperContent(PopperContentContext context) { _context = context; }

    internal double ArrowX { get; private set; }
    internal double ArrowY { get; private set; }
    [Inject]
    IJSRuntime JSRuntime { get; set; } = default!;
    string? StyleValue => new StyleBuilder(Style)
        .AddStyle("transform", "translate(0, -200%)")
        .Build();

    public async ValueTask DisposeAsync()
    {
        _dotNetRef?.Dispose();
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
            if (_jsModule != null)
            {
                _dotNetRef ??= DotNetObjectReference.Create(this);
                _floatingInstance = await _jsModule.InvokeAsync<IJSObjectReference>(
                    "create",
                    _dotNetRef,
                    _floatingRef,
                    Ref,
                    _context.Side.ToString(),
                    _context.ShouldHideArrow);
            }
        }
    }
}
