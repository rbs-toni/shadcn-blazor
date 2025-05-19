using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace ShadcnBlazor;
public partial class DismissableLayer
{
    List<ElementReference> LayersRoot = new List<ElementReference>();
    List<ElementReference> LayersWithOutsidePointerEventsDisabled = new List<ElementReference>();
    List<ElementReference> Branches = new List<ElementReference>();

    internal void RegisterBranch(ElementReference branch)
    {
        if (Branches.Contains(branch) == false)
        {
            Branches.Add(branch);
        }
    }

    internal void UnregisterBranch(ElementReference branch)
    {
        if (Branches.Contains(branch))
        {
            Branches.Remove(branch);
        }
    }
}
