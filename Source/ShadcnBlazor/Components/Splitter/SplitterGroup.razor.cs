using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadcnBlazor;
public partial class SplitterGroup
{
    Dictionary<string, double> _panelIdToLastNotifiedSizeMap;
    Dictionary<string, double> _panelSizeBeforeCollapse;
    double _prevDelta;
    protected double[] _layout = [];
    readonly PanelGroupContext _context;

    public SplitterGroup()
    {
        _context = new PanelGroupContext()
        {
            GroupId = Identifier.NewId(),
        };
    }

    void SetLayout(double[] layout)
    {
        if (_layout != layout)
        {
            _layout = layout;
        }
    }
}
