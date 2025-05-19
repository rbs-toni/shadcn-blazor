using System;
using System.Linq;

namespace ShadcnBlazor;
public sealed class InternalTooltipContext : IDisposable
{
    readonly Tooltip _tooltip;

    public InternalTooltipContext(Tooltip tooltip)
    {
        _tooltip = tooltip ?? throw new ArgumentNullException(nameof(tooltip));
        _tooltip.OnStateChanged += HandleTooltipStateChange;
        ContentId = Identifier.NewId();
        ArrowId = Identifier.NewId();
    }

    public event Action? OnClose;
    public event Action? OnOpen;

    public string AnchorId => _tooltip.AnchorId;
    public string ArrowId { get; }
    public string ContentId { get; }
    public bool DisableHoverableContent => _tooltip.DisableHoverableContent;
    public bool ShowArrow => _tooltip.ShowArrow;

    public void Dispose() => _tooltip.OnStateChanged -= HandleTooltipStateChange;
    void HandleTooltipStateChange(bool isOpen)
    {
        if (isOpen)
        {
            NotifyOpen();
        }
        else
        {
            NotifyClose();
        }
    }
    void NotifyClose() => OnClose?.Invoke();
    void NotifyOpen() => OnOpen?.Invoke();
}
