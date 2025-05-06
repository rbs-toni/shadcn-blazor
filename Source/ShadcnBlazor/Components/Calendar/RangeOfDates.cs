namespace ShadcnBlazor;
public class RangeOfDates : RangeOf<DateTime>
{
    /// <inheritdoc cref="RangeOf{T}"/>
    public RangeOfDates() : base() { }

    /// <inheritdoc cref="RangeOf{T}"/>
    public RangeOfDates(DateTime? start, DateTime? end) : base(start, end) { }

    /// <summary>
    /// Returns all values between <see cref="RangeOf{T}.Start"/> and <see cref="RangeOf{T}.End"/>
    /// </summary>
    /// <returns></returns>
    public DateTime[] GetAllDates()
    {
        return GetRangeValues((min, max) =>
        {
            return Enumerable.Range(0, (max - min).Days + 1)
                             .Select(offset => min.AddDays(offset))
                             .ToArray();
        });
    }

    public override string ToString()
    {
        return $"From {Start?.ToString("yyyy-MM-dd")} to {End?.ToString("yyyy-MM-dd")}.";
    }
}
