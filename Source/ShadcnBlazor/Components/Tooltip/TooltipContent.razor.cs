using System;
using System.Linq;

namespace ShadcnBlazor;
public partial class TooltipContent
{
    string? ClassValue => new CssBuilder(Class).AddClass(DefaultClasses).Build();
    string? StyleValue => new StyleBuilder(Style)
        .AddStyle("--blazor-tooltip-content-transform-origin", "--blazor-popper-transform-origin")
        .AddStyle("--blazor-tooltip-content-available-width", "--blazor-popper-available-width")
        .AddStyle("--blazor-tooltip-content-available-height", "--blazor-popper-available-height")
        .AddStyle("--blazor-tooltip-content-trigger-width", "--blazor-popper-anchor-width")
        .AddStyle("--blazor-tooltip-content-trigger-height", "--blazor-popper-anchor-height")
        .Build();

    protected override void OnInitialized()
    {
        if (TooltipContext != null)
        {
            TooltipContext.OnOpen += ()=>InvokeAsync(StateHasChanged);
        }
    }
}
