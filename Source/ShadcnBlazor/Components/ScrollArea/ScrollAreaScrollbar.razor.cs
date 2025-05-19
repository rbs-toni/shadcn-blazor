using System;
using System.Linq;

namespace ShadcnBlazor;
public partial class ScrollAreaScrollbar
{
    readonly ScrollAreaScrollbarContext _context;

    public ScrollAreaScrollbar()
    {
        _context = new(this);
    }

    internal bool IsHorizontal => Orientation == Orientation.Horizontal;

    protected override void OnParametersSet()
    {
        if (IsHorizontal)
        {
            if (RootContext != null)
            {
                RootContext.SetScrollbarXEnabled(true);
            }
        }
        else
        {
            if (RootContext != null)
            {
                RootContext.SetScrollbarYEnabled(true);
            }
        }
    }
}
