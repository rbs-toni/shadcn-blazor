using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace ShadcnBlazor;
public partial class Slider<TValue> : ShadcnInputBase<TValue>, IAsyncDisposable
    where TValue : INumber<TValue>
{
    const string JAVASCRIPT_FILE = "./_content/ShadcnBlazor/Components/Slider/Slider.razor.js";
    Debounce _debounce;
    TValue? _max;
    TValue? _min;
    bool _updateSliderThumb;

    public Slider()
    {
        _debounce = new Debounce();
    }

    /// <summary>
    /// Gets or sets the slider's maximum value.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public TValue? Max
    {
        get => _max;
        set
        {
            if (_max != value)
            {
                _max = value;
                _updateSliderThumb = true;
            }
        }
    }
    /// <summary>
    /// Gets or sets the slider's minimal value.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public TValue? Min
    {
        get => _min;
        set
        {
            if (_min != value)
            {
                _min = value;
                _updateSliderThumb = true;
            }
        }
    }
    /// <summary>
    /// Gets or sets the orientation of the slider. See <see cref="AspNetCore.Components.Orientation"/>
    /// </summary>
    [Parameter]
    public Orientation Orientation { get; set; } = Orientation.Horizontal;
    /// <summary>
    /// Gets or sets the slider's step value.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public TValue? Step { get; set; }
    public override TValue? Value
    {
        get => base.Value;
        set
        {
            if (base.Value != value)
            {
                base.Value = value;
                _updateSliderThumb = true;
            }
        }
    }
    protected override string? ClassValue
    {
        get { return new CssBuilder(base.ClassValue).AddClass(Orientation.ToAttributeValue() ?? "horizontal").Build(); }
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
            Module ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
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
}
