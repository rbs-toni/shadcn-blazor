using Microsoft.AspNetCore.Components;
using System;
using System.Linq;

namespace ShadcnBlazor;
public partial class PopperRoot
{
    readonly PopperRootContext _context;
    public PopperRoot()
    {
        _context = new PopperRootContext(this);
    }

    [Parameter]
    public RenderFragment<PopperRootContext>? ChildContent { get; set; }
}
