using System;
using System.Linq;
using System.Numerics;

namespace ShadcnBlazor;
public class SliderContext<TValue> where TValue : INumber<TValue>
{
    readonly Slider<TValue> _slider;

    public SliderContext(Slider<TValue> slider)
    {
        _slider = slider;
    }
    public Orientation Orientation => _slider.Orientation;
    public bool Disabled => _slider.Disabled;
    public TValue? Min => _slider.Min;
    public TValue? Max => _slider.Max;
    public ThumbAlignment ThumbAlignment => _slider.ThumbAlignment;
}

internal class SliderOrientationContext
{
    public SlideSide StartEdge { get; set; }
    public SlideSide EndEdge { get; set; }
    public string? Size { get; set; }
    public int Number { get; set; }
}

enum SlideSide
{
    Top,
    Right,
    Bottom,
    Left
}


