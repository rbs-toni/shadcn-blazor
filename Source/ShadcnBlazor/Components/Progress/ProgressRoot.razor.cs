using System;
using System.Linq;

namespace ShadcnBlazor;
public partial class ProgressRoot
{
    readonly ProgressRootContext _context;

    public ProgressRoot()
    {
        _context = new(this);
    }
}
