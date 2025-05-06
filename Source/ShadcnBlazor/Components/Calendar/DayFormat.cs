using System.ComponentModel;

namespace ShadcnBlazor;

/// <summary>
/// Defines the format of days shown in a <see cref="FluentCalendar"/> component.
/// </summary>
public enum DayFormat
{
    /// <summary>
    /// The day format uses 2 digits.
    /// </summary>
    [Description("2-digit")]
    TwoDigit,

    /// <summary>
    /// The day format is numeric.
    /// </summary>
    Numeric
}
