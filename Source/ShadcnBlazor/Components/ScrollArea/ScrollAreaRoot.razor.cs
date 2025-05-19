using System;
using System.Linq;

namespace ShadcnBlazor;
public partial class ScrollAreaRoot
{
    readonly ScrollAreaRootContext _context;

    public ScrollAreaRoot()
    {
        _context = new(this);
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            _context.SetScrollArea(Ref);
        }
    }
}
