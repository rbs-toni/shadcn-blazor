using Microsoft.AspNetCore.Components;
using System;
using System.Linq;

namespace ShadcnBlazor;
public partial class Teleport
{
    bool _isTeleported;
    bool _previousCondition;
    RenderFragment? _teleportedContent;

    public void Dispose()
    {
        if (_teleportedContent != null)
        {
            Teleporter.Remove(_teleportedContent);
        }
        GC.SuppressFinalize(this);
    }
    protected override void OnInitialized()
    {
        _previousCondition = If;
        UpdateTeleportState();
    }
    protected override void OnParametersSet()
    {
        if (If != _previousCondition)
        {
            _previousCondition = If;
            UpdateTeleportState();
        }
    }
    void UpdateTeleportState()
    {
        if (If && !_isTeleported && ChildContent != null)
        {
            _teleportedContent = ChildContent;
            Teleporter.Add(_teleportedContent);
            _isTeleported = true;
            OnTeleported.InvokeAsync();
        }
        else if (!If && _isTeleported && _teleportedContent != null)
        {
            Teleporter.Remove(_teleportedContent);
            _isTeleported = false;
            OnRemoved.InvokeAsync();
        }
    }
}
