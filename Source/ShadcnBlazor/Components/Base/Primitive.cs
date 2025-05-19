using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace ShadcnBlazor;
public class Primitive : ShadcnComponentBase
{
    [Parameter]
    public string As { get; set; } = "div";

    [Parameter]
    public bool StopPropagation { get; set; }

    [Parameter]
    public bool PreventDefault { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        base.BuildRenderTree(builder);

        var seq = 0;

        builder.OpenElement(seq++, As);

        builder.AddMultipleAttributes(seq++, Attributes);

        builder.AddAttribute(seq++, "class", Class);
        builder.AddAttribute(seq++, "style", Style);

        builder.AddEventStopPropagationAttribute(seq++, "onclick", StopPropagation);
        builder.AddEventPreventDefaultAttribute(seq++, "onclick", PreventDefault);

        builder.AddElementReferenceCapture(seq++, async capRef =>
        {
            Ref = capRef;
            await RefChanged.InvokeAsync(Ref);
        });

        builder.AddContent(seq++, ChildContent);
        builder.CloseElement();
    }
}
