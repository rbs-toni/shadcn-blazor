using System;
using System.Linq;

namespace ShadcnBlazor;
public partial class Collapsible
{
    readonly CollapsibleContext _context;

    public Collapsible()
    {
        _context = new(this);
    }
}
