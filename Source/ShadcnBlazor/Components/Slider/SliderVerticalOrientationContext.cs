using System;
using System.Linq;
using System.Numerics;

namespace ShadcnBlazor;
class SliderVerticalOrientationContext<TValue>: ISliderOrientationContext where TValue : INumber<TValue>
{
    readonly SliderVertical<TValue> _sliderVertical;

    public SliderVerticalOrientationContext(SliderVertical<TValue> sliderVertical)
    {
        _sliderVertical = sliderVertical;
    }

    public SlideSide StartEdge => _sliderVertical.IsSlidingFromBottom ? SlideSide.Bottom : SlideSide.Top;
    public SlideSide EndEdge => _sliderVertical.IsSlidingFromBottom ? SlideSide.Top : SlideSide.Bottom;
    public string? Size => "height";
    public int Direction => _sliderVertical.IsSlidingFromBottom ? 1 : -1;
}
