using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace ShadcnBlazor;
public partial class SliderVertical<TValue> : IAsyncDisposable where TValue : INumber<TValue>
{
    public static readonly Dictionary<SlideDirection, string[]> BackKeys = new()
    {
        [SlideDirection.FromLeft] = ["Home", "PageDown", "ArrowDown", "ArrowLeft"],
        [SlideDirection.FromRight] = ["Home", "PageDown", "ArrowDown", "ArrowRight"],
        [SlideDirection.FromBottom] = ["Home", "PageDown", "ArrowDown", "ArrowLeft"],
        [SlideDirection.FromTop] = ["Home", "PageDown", "ArrowUp", "ArrowLeft"]
    };
    readonly SliderVerticalOrientationContext<TValue> _orientationContext;
    IJSObjectReference _jsModule;
    double _offsetPosition;
    DomRect? _rect;

    public SliderVertical()
    {
        _orientationContext = new(this);
    }

    internal bool IsSlidingFromBottom => (Direction == Direction.LTR && !Inverted) || (Direction != Direction.LTR && Inverted);
    [Inject]
    IJSRuntime JSRuntime { get; set; } = default!;
    string? StyleValue => new StyleBuilder()
        .AddStyle("--slider-thumb-transform", "translateY(-50%)", "translateY(50%)", IsSlidingFromBottom == false && RootContext?.ThumbAlignment == ThumbAlignment.Overflow)
        .Build();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/ShadcnBlazor/Components/Slider/SliderHorizontal.razor.js");
        }
    }
    async Task<DomRect> GetBoundingClientRectAsync(string id)
    {
        if (_jsModule != null)
        {
            return await _jsModule.InvokeAsync<DomRect>("getBoundingClientRect", id);
        }

        return default!;
    }
    async Task<ElementInfo> GetElementInfoAsync(string id)
    {
        if (_jsModule != null)
        {
            return await _jsModule.InvokeAsync<ElementInfo>("getBoundingClientRect", id);
        }

        return default!;
    }
    async Task<double> GetValueFromPointerEventAsync(PointerEventArgs e, bool slideStart)
    {
        if (RootContext != null)
        {
            var rect = await ElementService.GetBoundingClientRectAsync(Ref);
            var thumbId = RootContext.ThumbElementId;
            var thumbRect = await GetBoundingClientRectAsync(thumbId);
            var thumbInfo = await GetElementInfoAsync(thumbId);
            var thumbHeight = RootContext.ThumbAlignment == ThumbAlignment.Contain ? thumbInfo.ClientHeight : 0;
            if (!slideStart && RootContext.ThumbAlignment == ThumbAlignment.Contain)
            {
                _offsetPosition = e.ClientY - thumbRect.Top;
            }
            var min = double.CreateChecked<TValue>(Min);
            var max = double.CreateChecked<TValue>(Max);
            var input = (0, rect.Height - thumbHeight);
            var output = IsSlidingFromBottom ? (max, min) : (min, max);
            var value = ScrollUtils.LinearScale(input, output);
            var position = slideStart ? e.ClientY - rect.Top - (thumbHeight / 2) : e.ClientY - rect.Top - _offsetPosition;
            _rect = rect;
            return value(position);
        }
        return default;
    }
    void OnEndKeyDownHandler(KeyboardEventArgs args)
    {
        OnEndKeyDown.InvokeAsync(args);
    }
    void OnHomeKeyDownHandler(KeyboardEventArgs args)
    {
        OnHomeKeyDown.InvokeAsync(args);
    }
    void OnSlideEndHandler(PointerEventArgs args)
    {
        _rect = null;
        _offsetPosition = 0;
        OnSlideEnd.InvokeAsync();
    }
    async Task OnSlideMoveHandlerAsync(PointerEventArgs args)
    {
        var value = await GetValueFromPointerEventAsync(args, false);
        await OnSlideMove.InvokeAsync(value);
    }
    async Task OnSlideStartHandlerAsync(PointerEventArgs args)
    {
        var value = await GetValueFromPointerEventAsync(args, true);
        await OnSlideStart.InvokeAsync(value);
    }
    void OnStepKeyDownHandler(KeyboardEventArgs e)
    {
        var slideDirection = IsSlidingFromBottom ? SlideDirection.FromBottom : SlideDirection.FromTop;
        var isBackKey = BackKeys[slideDirection].Contains(e.Key);
        OnStepKeyDown.InvokeAsync((e, isBackKey ? -1 : 1));
    }

    public async ValueTask DisposeAsync()
    {
        if (_jsModule != null)
        {
            await _jsModule.DisposeAsync();
        }
    }
}
