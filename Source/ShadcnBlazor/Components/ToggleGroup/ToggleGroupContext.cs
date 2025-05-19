using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadcnBlazor;
public class ToggleGroupContext
{
    private readonly ToggleGroup _group;

    public ToggleGroupContext(ToggleGroup group)
    {
        _group = group;
    }
}
