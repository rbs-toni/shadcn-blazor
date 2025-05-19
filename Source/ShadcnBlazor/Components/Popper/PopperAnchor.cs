using Microsoft.AspNetCore.Components;
using System;
using System.Linq;

namespace ShadcnBlazor;
public class PopperAnchor : Primitive
{
    [CascadingParameter]
    PopperRootContext? Context { get; set; }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            if (Context != null)
            {
                Context.OnAnchorChanged.InvokeAsync(Ref);
            }
        }
    }
}
