using System;
using System.Linq;
using System.Numerics;
using Microsoft.AspNetCore.Components;

namespace ShadcnBlazor;
public class SliderRootContext<TValue> where TValue : INumber<TValue>
{
    readonly SliderRoot<TValue> _slider;
    string _thumbElementId;

    public SliderRootContext(SliderRoot<TValue> slider)
    {
        _slider = slider;
    }

    public event Action? OnThumbElementsChanged;

    public bool Disabled => _slider.Disabled;
    public TValue? Max => _slider.Max;
    public TValue? Min => _slider.Min;
    public Orientation Orientation => _slider.Orientation;
    public ThumbAlignment ThumbAlignment => _slider.ThumbAlignment;
    public string ThumbElementId => _thumbElementId;
    public TValue? Value => _slider.Value;
    public event Action OnValueChanged;

    public void NotifyValueChanged()
    {
        OnValueChanged?.Invoke();
    }
    public void RegisterThumbElement(string thumbId)
    {
        _thumbElementId = thumbId;
        OnThumbElementsChanged?.Invoke();
    }
}
