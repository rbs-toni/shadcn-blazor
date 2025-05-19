using System;
using System.Linq;

namespace ShadcnBlazor;
public class InternalSwitchContext
{
    readonly Switch _switch;

    public InternalSwitchContext(Switch @switch)
    {
        _switch = @switch;
    }

    public bool Disabled => _switch.Disabled;
    public bool Value => _switch.Value;
    public string ValueAsString => Value == true ? "checked" : "unchecked";

    public void ToggleCheck()
    {
        _switch.ToggleCheck();
        NotifyStateHasChanged();
    }
    public void NotifyStateHasChanged()
    {
        OnStateChanged?.Invoke();
    }
    public event Action? OnStateChanged;
}
