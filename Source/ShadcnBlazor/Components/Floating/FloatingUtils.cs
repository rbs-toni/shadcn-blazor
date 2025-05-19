using System;
using System.Linq;

namespace ShadcnBlazor;
static class FloatingUtils
{
    public static string? BuildPlacement(FloatingSide side, FloatingAlign align)
    {
        var _side = side.ToAttributeValue(false);
        var _align = align.ToAttributeValue(false);

        return _align != "center" ? $"{_side}-{_align}" : _side;
    }
}
