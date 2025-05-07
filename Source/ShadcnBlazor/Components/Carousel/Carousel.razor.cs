using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.Linq;
using System.Text.Json;

namespace ShadcnBlazor;

public partial class Carousel : ShadcnComponentBase
{
    IJSObjectReference? _embla;

    [Inject]
    IJSRuntime JSRuntime { get; set; } = default!;

    IJSObjectReference? _jsModule;
    const string JSFile = "./_content/ShadcnBlazor/Components/Carousel/Carousel.razor.js";

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await LoadJSModuleAsync();
        }
    }

    protected bool JSLoaded { get; set; }

    DotNetObjectReference<Carousel>? _dotNetObject;

    internal async Task InitAsync(ElementReference elementReference)
    {
        if (_embla == null)
        {
            await LoadJSModuleAsync();
            if (_jsModule != null)
            {
                _embla = await _jsModule.InvokeAsync<IJSObjectReference>(
                    "embla.init",
                    elementReference,
                    CarouselOptions,
                    _dotNetObject,
                    nameof(OnSelect));
            }
        }
    }

    async Task LoadJSModuleAsync()
    {
        if (!JSLoaded)
        {
            _dotNetObject ??= DotNetObjectReference.Create(this);
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JSFile);
            if (_jsModule == null)
            {
                JSLoaded = true;
            }
        }
    }

    internal async Task ScrollNextAsync(bool? jump = null)
    {
        if (_embla != null)
        {
            await LoadJSModuleAsync();
            await _jsModule.InvokeVoidAsync("embla.scrollNext", _embla, jump);
        }
    }

    internal async Task ScrollPrevAsync(bool? jump = null)
    {
        if (_embla != null)
        {
            await LoadJSModuleAsync();
            await _jsModule.InvokeVoidAsync("embla.scrollPrev", _embla, jump);
        }
    }

    [JSInvokable]
    public void OnSelect(CarouselStateEventArg args)
    {
        CanScrollNext = args.CanScrollNext;
        CanScrollPrev = args.CanScrollPrev;
        if (OnStateChanged != null)
        {
            OnStateChanged();
        }
    }

    public event Action? OnStateChanged;

    public bool CanScrollNext { get; protected set; }

    public bool CanScrollPrev { get; protected set; }
}

public record CarouselStateEventArg()
{
    public bool CanScrollNext { get; set; }

    public bool CanScrollPrev { get; set; }
}
