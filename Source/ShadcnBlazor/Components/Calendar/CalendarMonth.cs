namespace ShadcnBlazor;
/// <summary>
/// Computes the properties of a month in the calendar.
/// </summary>
class CalendarMonth
{
    readonly Calendar _calendar;
    readonly bool _isInDisabledList;

    /// <summary>
    /// Initializes a new instance of the <see cref="CalendarMonth"/> class.
    /// </summary>
    /// <param name="calendar"></param>
    /// <param name="month"></param>
    internal CalendarMonth(Calendar calendar, DateTime month)
    {
        _calendar = calendar;
        Month = month.GetDay(_calendar.Culture) == 1 ? month : month.StartOfMonth(_calendar.Culture);

        if (calendar.DisabledCheckAllDaysOfMonthYear)
        {
            _isInDisabledList = calendar.AllDaysAreDisabled(month.StartOfMonth(_calendar.Culture), month.EndOfMonth(_calendar.Culture));
        }
        else
        {
            _isInDisabledList = calendar.DisabledDateFunc?.Invoke(Month) ?? false;
        }
    }

    /// <summary>
    /// Whether the month is disabled.
    /// </summary>
    public bool IsDisabled => _isInDisabledList;
    /// <summary>
    /// Whether the month is readonly.
    /// </summary>
    public bool IsReadOnly => _isInDisabledList || _calendar.ReadOnly;
    /// <summary>
    /// Whether the month is selected by the user
    /// </summary>
    public bool IsSelected => Month.GetYear(_calendar.Culture) == _calendar.Value?.GetYear(_calendar.Culture) && Month.GetMonth(_calendar.Culture) == _calendar.Value?.GetMonth(_calendar.Culture);
    /// <summary>
    /// Current month (day is always 1)
    /// </summary>
    public DateTime Month { get; }
    /// <summary>
    /// Gets the identifier of the month in the format yyyy-MM.
    /// </summary>
    public string MonthIdentifier => Month.ToString("yyyy-MM", _calendar.Culture);
    /// <summary>
    /// Gets the title of the month in the format [month] [year].
    /// </summary>
    public string Title => $"{_calendar.CalendarExtended.GetMonthName(Month)} {Month.GetYear(_calendar.Culture):0000}";
}
