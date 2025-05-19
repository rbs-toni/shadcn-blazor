using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Threading.Tasks;

namespace ShadcnBlazor;
public partial class SliderRoot<TValue> : ShadcnInputBase<TValue>, IAsyncDisposable
    where TValue : INumber<TValue>
{
    const string JS_FILE = "./_content/ShadcnBlazor/Components/Slider/SliderRoot.razor.js";
    SliderRootContext<TValue> _context;
    Debounce _debounce;
    TValue? _max;
    TValue? _min;
    bool _updateSliderThumb;

    public SliderRoot()
    {
        _debounce = new Debounce();
        _context = new(this);
    }

    protected override string? ClassValue
    {
        get
        {
            return new CssBuilder(base.ClassValue)
                .AddClass(Orientation.ToAttributeValue() ?? "horizontal")
                .Build();
        }
    }
    [Inject]
    IJSRuntime JSRuntime { get; set; } = default!;
    IJSObjectReference? Module { get; set; }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (Module is not null)
            {
                await Module.DisposeAsync();
            }
        }
        catch (Exception ex) when (ex is JSDisconnectedException ||
                                   ex is OperationCanceledException)
        {
            // The JSRuntime side may routinely be gone already if the reason we're disposing is that
            // the client disconnected. This is not an error.
        }
    }
    /// <summary>
    /// Formats the value as a string. Derived classes can override this to determine the formatting used for <c>CurrentValueAsString</c>.
    /// </summary>
    /// <param name = "value">The value to format.</param>
    /// <returns>A string representation of the value.</returns>
    protected override string? FormatValueAsString(TValue? value)

    {
        // Avoiding a cast to IFormattable to avoid boxing.
        return value switch
        {
            null => null,
            int @int => BindConverter.FormatValue(@int, CultureInfo.InvariantCulture),
            long @long => BindConverter.FormatValue(@long, CultureInfo.InvariantCulture),
            short @short => BindConverter.FormatValue(@short, CultureInfo.InvariantCulture),
            float @float => BindConverter.FormatValue(@float, CultureInfo.InvariantCulture),
            double @double => BindConverter.FormatValue(@double, CultureInfo.InvariantCulture),
            decimal @decimal => BindConverter.FormatValue(@decimal, CultureInfo.InvariantCulture),
            _ => throw new InvalidOperationException($"Unsupported type {value.GetType()}"),
        };
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            Module ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JS_FILE);
        }
        else
        {
            if (_updateSliderThumb)
            {
                _updateSliderThumb = false;
                if (Module is not null)
                {
                    _debounce.Run(100, async () =>
                    {
                        await Module!.InvokeVoidAsync("updateSlider", Ref);
                    });
                }
            }
        }
    }
    protected override void OnParametersSet()
    {
        ArgumentNullException.ThrowIfNull(Min, nameof(Min));
        ArgumentNullException.ThrowIfNull(Max, nameof(Max));
        ArgumentNullException.ThrowIfNull(Step, nameof(Step));
    }
    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        if (BindConverter.TryConvertTo(value, CultureInfo.InvariantCulture, out result))
        {
            validationErrorMessage = null;
            return true;
        }
        else
        {
            validationErrorMessage = string.Format(CultureInfo.InvariantCulture, "The {0} field must be a number.", DisplayName ?? (FieldBound ? FieldIdentifier.FieldName : "(unknown)"));
            return false;
        }
    }

    TValue _valueBeforeSlideSlideStart;

    void OnPointerDown()
    {
        if (!Disabled)
        {
            _valueBeforeSlideSlideStart = CurrentValue;
        }
    }

    async Task OnSlideStart(double value)
    {
        if (!Disabled)
        {
            await ChangeHandlerAsync(new ChangeEventArgs()
            {
                Value = value,
            });
            _context.NotifyValueChanged();
        }
    }

    async Task OnSlideMove(double value)
    {
        if (!Disabled)
        {
            await ChangeHandlerAsync(new ChangeEventArgs()
            {
                Value = value,
            });
            _context.NotifyValueChanged();
        }
    }

    async Task OnSlideEnd()
    {
        if (!Disabled)
        {
            var prevValue = _valueBeforeSlideSlideStart;
            var nextValue = CurrentValue;
        }
    }
}
