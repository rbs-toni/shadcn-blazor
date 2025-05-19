using System;
using System.Linq;

namespace ShadcnBlazor;
public partial class Tabs
{
    TabsContext _context;
    public Tabs()
    {
        Id = Identifier.NewId();
        _context = new(this);
    }
}
