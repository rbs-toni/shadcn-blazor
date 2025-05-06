using Microsoft.AspNetCore.Components;

namespace ShadcnBlazor;

public class ForwardRef
{
    private ElementReference _current = default!;

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
