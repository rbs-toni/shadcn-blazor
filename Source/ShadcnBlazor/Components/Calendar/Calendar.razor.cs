using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ShadcnBlazor;
using System.Diagnostics.CodeAnalysis;

namespace ShadcnBlazor;

/// <summary>
/// Calendar based on https://github.com/microsoft/fluentui/blob/master/packages/web-components/src/calendar/.
/// </summary>
public partial class Calendar : CalendarBase
{
    public static string ArrowDown = "<svg width=\"24\" height=\"24\" viewBox=\"0 0 24 24\" fill=\"var(--neutral-fill-strong-focus)\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M19.8 13.27a.75.75 0 00-1.1-1.04l-5.95 6.25V3.75a.75.75 0 10-1.5 0v14.73L5.3 12.23a.75.75 0 10-1.1 1.04l7.08 7.42a1 1 0 001.44 0l7.07-7.42z\"/></svg>";
    public static string ArrowUp = "<svg width=\"24\" height=\"24\" viewBox=\"0 0 24 24\" fill=\"var(--neutral-fill-strong-focus)\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M4.2 10.73a.75.75 0 001.1 1.04l5.95-6.25v14.73a.75.75 0 001.5 0V5.52l5.95 6.25a.75.75 0 001.1-1.04l-7.08-7.42a1 1 0 00-1.44 0L4.2 10.73z\"/></svg>";
    internal CalendarViews _pickerView = CalendarViews.Days;
    readonly CalendarExtended? _calendarExtended;
    readonly RangeOfDates _rangeSelector = new();
    readonly RangeOfDates _rangeSelectorMouseOver = new();
    readonly List<DateTime> _selectedDatesMouseOver = new();
    VerticalPosition _animationRunning = VerticalPosition.Unset;
    DateTime? _pickerMonth;

    /// <summary>
    /// Gets ot sets if the calendar items are animated during a period change. By default, the animation is enabled for
    /// Months views, but disabled for Days and Years view.
    /// </summary>
    [Parameter]
    public bool? AnimatePeriodChanges { get; set; }

    /// <summary>
    /// Defines the appearance of a Day cell.
    /// </summary>
    [Parameter]
    public RenderFragment<CalendarDay>? DaysTemplate { get; set; }

    /// <summary>
    /// Gets or sets the current month of the date picker (two-way bindable). This changes when the user browses through
    /// the calendar. The month is represented as a DateTime which is always the first day of that month. You can also
    /// set this to determine which month is displayed first. If not set, the current month is displayed.
    /// </summary>
    [Parameter]
    public virtual DateTime PickerMonth
    {
        get { return (_pickerMonth ?? Value ?? DateTime.Today).StartOfMonth(Culture); }

        set
        {
            var month = value.StartOfMonth(Culture);

            if (month == _pickerMonth)
            {
                return;
            }

            _pickerMonth = month;
            PickerMonthChanged.InvokeAsync(month);
        }
    }

    /// <summary>
    /// Fired when the display month changes.
    /// </summary>
    [Parameter]
    public virtual EventCallback<DateTime> PickerMonthChanged { get; set; }

    /// <summary>
    /// Fired when the selected mouse over change, to display the future range of dates.
    /// </summary>
    [Parameter]
    public Func<DateTime, IEnumerable<DateTime>>? SelectDatesHover { get; set; }

    /// <summary>
    /// Gets or sets the list of all selected dates, only when <see cref="SelectMode"/> is set to <see
    /// cref="CalendarSelectMode.Range"/> or <see cref="CalendarSelectMode.Multiple"/>.
    /// </summary>
    [Parameter]
    public IEnumerable<DateTime> SelectedDates { get; set; } = new List<DateTime>();

    /// <summary>
    /// Fired when the selected dates change.
    /// </summary>
    [Parameter]
    public EventCallback<IEnumerable<DateTime>> SelectedDatesChanged { get; set; }

    /// <summary>
    /// Gets or sets the way the user can select one or more dates
    /// </summary>
    [Parameter]
    public CalendarSelectMode SelectMode { get; set; } = CalendarSelectMode.Single;

    /// <summary>
    /// All days of this current month.
    /// </summary>
    internal CalendarExtended CalendarExtended => _calendarExtended ?? new CalendarExtended(Culture, PickerMonth);

    /// <summary/>
    bool CanBeAnimated => AnimatePeriodChanges ?? (View != CalendarViews.Days && View != CalendarViews.Years);

    /// <summary>
    /// Check if all days between two dates are disabled.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    internal bool AllDaysAreDisabled(DateTime start, DateTime end)
    {
        if (DisabledDateFunc is null)
        {
            return false;
        }

        for (var day = start; day <= end; day = day.AddDays(1))
        {
            if (!DisabledDateFunc.Invoke(day))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Gets titles to use in the calendar.
    /// </summary>
    /// <returns></returns>
    internal CalendarTitles GetTitles() { return new CalendarTitles(this); }

    /// <summary/>
    protected virtual async Task OnSelectDayHandlerAsync(DateTime value, bool dayDisabled)
    {
        if (!dayDisabled)
        {
            switch (SelectMode)
            {
                // Single selection
                case CalendarSelectMode.Single:
                    await OnSelectedDateHandlerAsync(value);
                    break;

                // Multiple selection
                case CalendarSelectMode.Multiple:

                    if (SelectDatesHover is null)
                    {
                        if (SelectedDates.Contains(value))
                        {
                            SelectedDates = SelectedDates.Where(i => i != value);
                        }
                        else
                        {
                            SelectedDates = SelectedDates.Append(value);
                        }

                        if (SelectedDatesChanged.HasDelegate)
                        {
                            await SelectedDatesChanged.InvokeAsync(SelectedDates);
                        }
                    }
                    else
                    {
                        var range = SelectDatesHover.Invoke(value);

                        SelectedDates = range.Where(day => DisabledDateFunc != null ? !DisabledDateFunc(day) : true);

                        if (SelectedDatesChanged.HasDelegate)
                        {
                            await SelectedDatesChanged.InvokeAsync(SelectedDates);
                        }
                    }

                    break;

                // Range of dates
                case CalendarSelectMode.Range:

                    bool resetRange = (_rangeSelector.IsValid() || _rangeSelector.IsSingle()) &&
                        _rangeSelector.Includes(value);

                    // Reset the selection
                    if (resetRange)
                    {
                        _rangeSelector.Clear();
                        _rangeSelectorMouseOver.Clear();
                    }

                    // End the selection
                    else if (_rangeSelector.Start is not null && _rangeSelector.End is null)
                    {
                        _rangeSelector.End = value;
                    }

                    // Start and close a pre-selection
                    else if (SelectDatesHover is not null)
                    {
                        var range = SelectDatesHover.Invoke(value);

                        _rangeSelector.Start = range.Min();
                        _rangeSelector.End = range.Max();
                    }

                    // Start the selection
                    else
                    {
                        _rangeSelector.Start = value;
                        _rangeSelector.End = null;

                        await OnSelectDayMouseOverAsync(value, dayDisabled: false);
                    }

                    SelectedDates = _rangeSelector.GetAllDates()
                        .Where(day => DisabledDateFunc != null ? !DisabledDateFunc(day) : true);

                    if (SelectedDatesChanged.HasDelegate)
                    {
                        await SelectedDatesChanged.InvokeAsync(SelectedDates);
                    }
                    break;
            }
        }
    }

    /// <summary/>
    protected override bool TryParseValueFromString(
        string? value,
        out DateTime? result,
        [NotNullWhen(false)] out string? validationErrorMessage)
    {
        BindConverter.TryConvertTo(value, Culture, out result);
        validationErrorMessage = null;
        return true;
    }

    /// <summary/>
    string GetAnimationClass(string existingClass) => CanBeAnimated
        ? _animationRunning switch
        {
            VerticalPosition.Top => $"{existingClass} animation-running-up",
            VerticalPosition.Bottom => $"{existingClass} animation-running-down",
            _ => $"{existingClass} animation-none"
        }
        : existingClass;

    /// <summary>
    /// Returns the class name to display a day (day, inactive, today).
    /// </summary>
    /// <param name="day"></param>
    /// <returns></returns>
    CalendarDay GetDayProperties(DateTime day) => new(this, day);

    /// <summary>
    /// Returns the class name to display a month (month, inactive, disable).
    /// </summary>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <returns></returns>
    CalendarMonth GetMonthProperties(int? year, int? month) => new(
        this,
        Culture.Calendar
            .ToDateTime(year ?? PickerMonth.GetYear(Culture), month ?? PickerMonth.GetMonth(Culture), 1, 0, 0, 0, 0));

    /// <summary/>
    (bool IsMultiple, DateTime Min, DateTime Max, bool InProgress) GetMultipleSelection()
    {
        bool inProgress = SelectDatesHover is not null;

        if (SelectedDates == null || !SelectedDates.Any())
        {
            return (false, DateTime.MinValue, DateTime.MinValue, inProgress);
        }

        if (SelectDatesHover is null)
        {
            inProgress = !_rangeSelector.IsValid();
        }
        else
        {
            inProgress = _rangeSelectorMouseOver.IsValid();
        }

        return (
            (SelectMode == CalendarSelectMode.Multiple || SelectMode == CalendarSelectMode.Range) &&
            SelectedDates.Count() > 1,
            SelectedDates.Min(),
            SelectedDates.Max(),
            inProgress
               );
    }

    /// <summary>
    /// Returns the class name to display a year (year, inactive, disable).
    /// </summary>
    /// <param name="year"></param>
    /// <returns></returns>
    CalendarYear GetYearProperties(int? year) => new(
        this,
        Culture.Calendar.ToDateTime(year ?? PickerMonth.GetYear(Culture), 1, 1, 0, 0, 0, 0));

    /// <summary/>
    internal async Task OnNextButtonHandlerAsync(MouseEventArgs e)
    {
        await StartNewAnimationAsync(VerticalPosition.Top);

        switch (View)
        {
            case CalendarViews.Days:
                PickerMonth = PickerMonth.AddMonths(+1, Culture);
                break;

            case CalendarViews.Months:
                PickerMonth = PickerMonth.AddYears(+1, Culture);
                break;

            case CalendarViews.Years:
                PickerMonth = PickerMonth.AddYears(+12, Culture);
                break;
        }
    }

    /// <summary/>
    internal async Task OnPreviousButtonHandlerAsync(MouseEventArgs e)
    {
        await StartNewAnimationAsync(VerticalPosition.Bottom);

        switch (View)
        {
            case CalendarViews.Days:
                PickerMonth = PickerMonth.AddMonths(-1, Culture);
                break;

            case CalendarViews.Months:
                PickerMonth = PickerMonth.AddYears(-1, Culture);
                break;

            case CalendarViews.Years:
                PickerMonth = PickerMonth.AddYears(-12, Culture);
                break;
        }
    }

    /// <summary/>
    Task OnSelectDayMouseOverAsync(DateTime value, bool dayDisabled)
    {
        if (dayDisabled ||
            SelectMode == CalendarSelectMode.Single ||
            (_rangeSelector.IsSingle() && SelectDatesHover is null))
        {
            return Task.CompletedTask;
        }

        if (SelectDatesHover is null)
        {
            _rangeSelectorMouseOver.Start = _rangeSelector.Start ?? value;
            _rangeSelectorMouseOver.End = value;
        }
        else
        {
            var range = SelectDatesHover.Invoke(value);
            _rangeSelectorMouseOver.Start = range.Min();
            _rangeSelectorMouseOver.End = range.Max();
        }

        var days = DisabledDateFunc is null
            ? _rangeSelectorMouseOver.GetAllDates()
            : _rangeSelectorMouseOver.GetAllDates().Where(day => !DisabledDateFunc(day));

        _selectedDatesMouseOver.Clear();
        _selectedDatesMouseOver.AddRange(days);

        return Task.CompletedTask;
    }

    /// <summary/>
    async Task OnSelectMonthHandlerAsync(int year, int month, bool isReadOnly)
    {
        if (!isReadOnly)
        {
            var value = Culture.Calendar.ToDateTime(year, month, 1, 0, 0, 0, 0);
            await OnSelectedDateHandlerAsync(value);
        }
    }

    /// <summary/>
    async Task OnSelectYearHandlerAsync(int year, bool isReadOnly)
    {
        if (!isReadOnly)
        {
            var value = Culture.Calendar.ToDateTime(year, 1, 1, 0, 0, 0, 0);
            await OnSelectedDateHandlerAsync(value);
        }
    }

    /// <summary>
    /// Select a Month and come back to the Days view.
    /// </summary>
    /// <param name="month"></param>
    /// <returns></returns>
    async Task PickerMonthSelectAsync(DateTime? month)
    {
        PickerMonth = month ?? DateTime.Today;
        _pickerView = CalendarViews.Days;
        await Task.CompletedTask;
    }

    /// <summary>
    /// Select a Year and come back to the Months view.
    /// </summary>
    /// <param name="year"></param>
    /// <returns></returns>
    async Task PickerYearSelectAsync(DateTime? year)
    {
        PickerMonth = year ?? DateTime.Today;
        _pickerView = CalendarViews.Days;
        await Task.CompletedTask;
    }

    /// <summary/>
    async Task StartNewAnimationAsync(VerticalPosition position)
    {
        if (CanBeAnimated)
        {
            // Remove the current animation
            _animationRunning = VerticalPosition.Unset;
            await Task.Delay(1);
            StateHasChanged();

            // Start the new animation
            _animationRunning = position;
        }
    }

    /// <summary>
    /// Click on the Calendar Title to disply the Month or Year selector
    /// </summary>
    /// <param name="title"></param>
    /// <returns></returns>
    internal async Task TitleClickHandlerAsync(CalendarTitles title)
    {
        if (title.ReadOnly)
        {
            return;
        }

        switch (View)
        {
            // Days -> Months
            case CalendarViews.Days:
                _pickerView = CalendarViews.Months;
                break;

            // Months -> Years
            case CalendarViews.Months:
                _pickerView = CalendarViews.Years;
                break;
        }

        await Task.CompletedTask;
    }
}
