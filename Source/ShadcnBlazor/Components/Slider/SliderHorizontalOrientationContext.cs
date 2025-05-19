using System;
using System.Linq;
using System.Numerics;

namespace ShadcnBlazor;
class SliderHorizontalOrientationContext<TValue> : ISliderOrientationContext where TValue : INumber<TValue>
{
    readonly SliderHorizontal<TValue> _sliderHorizontal;

    public SliderHorizontalOrientationContext(SliderHorizontal<TValue> sliderHorizontal)
    {
        _sliderHorizontal = sliderHorizontal;
    }

    public int Direction => _sliderHorizontal.IsSlidingFromLeft ? 1 : -1;
    public SlideSide EndEdge => _sliderHorizontal.IsSlidingFromLeft ? SlideSide.Right : SlideSide.Left;
    public string? Size => "width";
    public SlideSide StartEdge => _sliderHorizontal.IsSlidingFromLeft ? SlideSide.Left : SlideSide.Right;
}
