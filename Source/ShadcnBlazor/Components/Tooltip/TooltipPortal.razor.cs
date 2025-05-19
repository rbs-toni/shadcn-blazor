using Microsoft.AspNetCore.Components;

namespace ShadcnBlazor;
public partial class TooltipPortal
{
    readonly string _tooltipPortalId = Identifier.NewId();
    bool _isContentRendered;
    bool _previousShow;
    RenderFragment? _renderedContent;

    protected override void OnParametersSet()
    {
        if (Show == _previousShow)
            return;

        _previousShow = Show;
        TryUpdateTeleport();
    }
    void TryUpdateTeleport()
    {
        if (Show)
        {
            if (_isContentRendered || ChildContent is null)
                return;
            _renderedContent = ChildContent;
            TooltipService.AddTooltip(_tooltipPortalId, _renderedContent);
            _isContentRendered = true;
            OnRendered.InvokeAsync();
        }
        else if (_isContentRendered)
        {
            TooltipService.RemoveTooltip(_tooltipPortalId);
            _isContentRendered = false;
            OnRemoved.InvokeAsync();
        }
    }
}
