namespace ShadcnBlazor;

public class PositionChangedEventArgsComparer : IEqualityComparer<PositionChangedEventArgs>
{
    private const double Epsilon = 0.05;

    public bool Equals(PositionChangedEventArgs? x, PositionChangedEventArgs? y)
    {
        if (ReferenceEquals(x, y))
            return true;
        if (x is null || y is null)
            return false;

        return AreClose(x.X, y.X) &&
               AreClose(x.Y, y.Y) &&
               string.Equals(x.Placement, y.Placement, StringComparison.Ordinal) &&
               string.Equals(x.Strategy, y.Strategy, StringComparison.Ordinal);
    }

    public int GetHashCode(PositionChangedEventArgs obj)
    {
        // Quantize the doubles to reduce hash inconsistency from precision issues
        int quantizedX = (int)Math.Round(obj.X / Epsilon);
        int quantizedY = (int)Math.Round(obj.Y / Epsilon);

        return HashCode.Combine(
            quantizedX,
            quantizedY,
            obj.Placement ?? string.Empty,
            obj.Strategy ?? string.Empty
        );
    }

    private static bool AreClose(double a, double b)
        => Math.Abs(a - b) < Epsilon;
}
