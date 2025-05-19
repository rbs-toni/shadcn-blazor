using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Linq;

namespace ShadcnBlazor;
public partial class SliderThumbImpl<TValue> : IAsyncDisposable
{
    DotNetObjectReference<SliderThumbImpl<TValue>>? _dotNet;
    bool _init;
    IJSObjectReference? _jsModule;
    Size? _size;
    IJSObjectReference? _thumbInstance;

    public SliderThumbImpl()
    {
        Id = Identifier.NewId();
    }

    [Inject]
    IJSRuntime JSRuntime { get; set; } = default!;
    string? Label => Value?.ToString() ?? string.Empty;
    double? MaxAsDouble => double.CreateChecked(RootContext.Max);
    double? MinAsDouble => double.CreateChecked(RootContext.Min);
    double OrientationSize => OrientationContext?.Size == "height" ? _size?.Height ?? 0 : _size?.Width ?? 0;
    double? Percentage => Value == null ? 0 : ScrollUtils.ConvertValueToPercentage(ValueAsDouble ?? 0, MinAsDouble ?? 0, MaxAsDouble ?? 100);
    double? ThumbInBoundsOffset
    {
        get
        {
            if (RootContext?.ThumbAlignment == ThumbAlignment.Overflow)
            {
                return 0;
            }
            else
            {
                return GetThumbInBoundsOffset(OrientationSize, Percentage ?? 0, OrientationContext?.Direction ?? 1);
            }
        }
    }
    TValue? Value => RootContext == null ? default : RootContext.Value;
    double? ValueAsDouble => double.CreateChecked(Value);

    public async ValueTask DisposeAsync()
    {
        if (_jsModule != null)
        {
            await _jsModule.DisposeAsync();
        }
        if (_thumbInstance != null)
        {
            await _thumbInstance.InvokeVoidAsync("dispose");
            await _thumbInstance.DisposeAsync();
        }
    }
    [JSInvokable]
    public void UseSize(Size size)
    {
        _size = size;
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            if (RootContext != null)
            {
                RootContext.RegisterThumbElement(Id);
            }
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/ShadcnBlazor/Components/Slider/SliderThumbImpl.razor.js");
        }
        if (_jsModule != null && !string.IsNullOrWhiteSpace(Ref.Id) && !_init)
        {
            _dotNet ??= DotNetObjectReference.Create(this);
            _thumbInstance = await _jsModule.InvokeAsync<IJSObjectReference>("useSize", Ref, _dotNet, nameof(UseSize));
            _init = true;
        }
    }
    double GetThumbInBoundsOffset(double width, double left, int direction)
    {
        var halfWidth = width / 2;
        var halfPercent = 50;
        var offset = ScrollUtils.LinearScale((0, halfPercent), (0, halfWidth));

        return (halfWidth - offset(left) * direction) * direction;
    }

    protected override void OnInitialized()
    {
        if (RootContext != null)
        {
            RootContext.OnValueChanged += () => InvokeAsync(StateHasChanged);
        }
    }

    public record Size
    {
        public double Height { get; set; }
        public double Width { get; set; }
    }
}
