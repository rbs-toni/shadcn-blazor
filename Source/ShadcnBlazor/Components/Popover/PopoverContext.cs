using System;
using System.Linq;

namespace ShadcnBlazor;
public class PopoverContext
{
    readonly Popover _popover;

    public PopoverContext(Popover popover)
    {
        _popover = popover;
    }

    public event Action? OnClosed;

    public void Close()
    {
        NotifyStateHasChanged();
    }
    void NotifyStateHasChanged()
    {
        OnClosed?.Invoke();
    }
}
