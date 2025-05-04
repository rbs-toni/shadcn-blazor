using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadcnBlazor;
class CollapsibleContext
{
    public string? ContentId { get; set; }
    public bool Disabled { get; set; }
    public bool Open { get; set; }
    public void ToggleOpen()
    {
        Open = !Open;
    }
}
