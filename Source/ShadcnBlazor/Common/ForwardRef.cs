using Microsoft.AspNetCore.Components;

namespace ShadcnBlazor;
public class ForwardRef
{
    ElementReference _current;

    public ElementReference Current
    {
        get => _current;
        set => Set(value);
    }

    public void Set(ElementReference value)
    {
        _current = value;
    }
}
