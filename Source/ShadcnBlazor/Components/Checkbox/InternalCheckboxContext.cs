using System;
using System.Linq;

namespace ShadcnBlazor;
class InternalCheckboxContext
{
    public bool Disabled { get; set; }
    public CheckboxChangeEventArgs? State { get; set; }
}
