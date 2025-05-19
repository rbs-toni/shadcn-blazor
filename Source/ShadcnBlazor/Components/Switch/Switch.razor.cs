using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ShadcnBlazor;
public partial class Switch : ShadcnInputBase<bool>
{
    readonly InternalSwitchContext _context;

    public Switch()
    {
        _context = new(this);
    }

    internal void ToggleCheck()
    {
        CurrentValue = !CurrentValue;
        _context.NotifyStateHasChanged();
    }
    protected override bool TryParseValueFromString(string? value, out bool result, [NotNullWhen(false)] out string? validationErrorMessage) => throw new NotSupportedException($"This component does not parse string inputs. Bind to the '{nameof(CurrentValue)}' property, not '{nameof(CurrentValueAsString)}'.");
}
