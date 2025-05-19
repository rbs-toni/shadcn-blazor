using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace ShadcnBlazor;
public abstract class ShadcnComponentBase : ComponentBase
{
    ILogger? _logger;
    ElementReference _ref;

    /// <summary>
    /// Gets or sets a collection of additional attributes that will be applied to the created element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? Attributes { get; set; }
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
    /// <summary>
    /// Optional CSS class names. If given, these will be included in the class attribute of the component.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }
    /// <summary>
    /// A reference to the enclosing component.
    /// </summary>
    [Parameter]
    public ForwardRef? ForwardRef { get; set; }
    /// <summary>
    /// Gets or sets the unique identifier.
    /// The value will be used as the HTML <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/id">global id attribute</see>.
    /// </summary>
    [Parameter]
    public string? Id { get; set; }
    /// <summary>
    /// Gets or sets the associated web component. 
    /// May be <see langword="null"/> if accessed before the component is rendered.
    /// </summary>
    [Parameter]
    public ElementReference Ref
    {
        get => _ref;
        set
        {
            _ref = value;
            ForwardRef?.Set(_ref);
        }
    }

    /// <summary>
    /// Gets or sets the associated web component. 
    /// May be <see langword="null"/> if accessed before the component is rendered.
    /// </summary>
    [Parameter]
    public EventCallback<ElementReference> RefChanged { get; set; }

    /// <summary>
    /// Optional in-line styles. If given, these will be included in the style attribute of the component.
    /// </summary>
    [Parameter]
    public string? Style { get; set; }
    [Parameter]
    public StyleType StyleType { get; set; } = StyleType.NewYork;
    protected bool IsDefaultStyle => GlobalStyle.HasValue
        ? GlobalStyle.Value == StyleType.Default
        : StyleType == StyleType.Default;
    protected ILogger Logger => _logger ??= LoggerFactory.CreateLogger(GetType());
    [CascadingParameter(Name = "Global Style")]
    StyleType? GlobalStyle { get; set; }
    [Inject]
    ILoggerFactory LoggerFactory { get; set; } = default!;

    protected string? GetId()
    {
        return string.IsNullOrEmpty(Id) ? null : Id;
    }
}
