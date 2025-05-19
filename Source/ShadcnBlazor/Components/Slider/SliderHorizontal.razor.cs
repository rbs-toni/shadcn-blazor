using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Linq;
using System.Numerics;
using System.Text.Json;

namespace ShadcnBlazor;
public partial class SliderHorizontal<TValue> : IAsyncDisposable where TValue : INumber<TValue>
{
    public static readonly Dictionary<SlideDirection, string[]> BackKeys = new()
    {
        [SlideDirection.FromLeft] = ["Home", "PageDown", "ArrowDown", "ArrowLeft"],
        [SlideDirection.FromRight] = ["Home", "PageDown", "ArrowDown", "ArrowRight"],
        [SlideDirection.FromBottom] = ["Home", "PageDown", "ArrowDown", "ArrowLeft"],
        [SlideDirection.FromTop] = ["Home", "PageDown", "ArrowUp", "ArrowLeft"]
    };
    readonly SliderHorizontalOrientationContext<TValue> _orientationContext;
    IJSObjectReference? _jsModule;
    double? _offsetPosition;
    DomRect? _rect;

    public SliderHorizontal()
    {
        _orientationContext = new(this);
        Id = Identifier.NewId();
    }

    internal bool IsSlidingFromLeft => Direction == Direction.LTR && !Inverted || Direction != Direction.LTR && Inverted;
    ElementReference CurrentRef => _forwardRef.Current;
    [Inject]
    IJSRuntime JSRuntime { get; set; } = default!;
    string? StyleValue => new StyleBuilder()
        .AddStyle("--slider-thumb-transform", "translateX(50%)", "translateX(-50%)", IsSlidingFromLeft == false && RootContext?.ThumbAlignment == ThumbAlignment.Overflow)
        .Build();

    public async ValueTask DisposeAsync()
    {
        if (_jsModule != null)
        {
            await _jsModule.DisposeAsync();
        }
    }

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

            var rect = _rect ?? await GetBoundingClientRectAsync(Id!);
            var thumbId = RootContext.ThumbElementId;
            var thumbRect = await GetBoundingClientRectAsync(thumbId);
            var thumbInfo = await GetElementInfoAsync(thumbId);
            var thumbWidth = RootContext.ThumbAlignment == ThumbAlignment.Contain ? thumbInfo.ClientWidth : 0;
            if (_offsetPosition == null && !slideStart && RootContext.ThumbAlignment == ThumbAlignment.Contain)
            {
                _offsetPosition = e.ClientX - thumbRect.Left;
            }
            var min = double.CreateChecked(Min);
            var max = double.CreateChecked(Max);
            var input = (0, rect.Width - thumbWidth);
            var output = IsSlidingFromLeft ? (min, max) : (max, min);
            var value = ScrollUtils.LinearScale(input, output);
            _rect = rect;
            var position = slideStart ? e.ClientX - rect.Left - (thumbWidth / 2) : e.ClientX - rect.Left - (_offsetPosition ?? 0);
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
        _offsetPosition = null;
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
        var slideDirection = IsSlidingFromLeft ? SlideDirection.FromLeft : SlideDirection.FromRight;
        var isBackKey = BackKeys[slideDirection].Contains(e.Key);
        OnStepKeyDown.InvokeAsync((e, isBackKey ? -1 : 1));
    }
}
