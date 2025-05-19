using System;
using System.Linq;

namespace ShadcnBlazor;

public static class ScrollAreaUtils
{
    const double EPSILON = 0.0001;

    public static double GetThumbOffsetFromScroll(
        double scrollPos,
        ScrollSize sizes,
        Direction direction = Direction.LTR)
    {
        var thumbsize = GetThumbSize(sizes);
        var scrollbarPadding = sizes.Scrollbar?.PaddingStart + sizes.Scrollbar?.PaddingEnd;
        var scrollbar = sizes.Scrollbar?.Size - scrollbarPadding;
        var maxScrollPos = sizes.Content - sizes.ViewPort;
        var maxThumbPos = scrollbar - thumbsize;
        var scrollClampRange = direction == Direction.LTR ? (0d, maxScrollPos) : (maxScrollPos * -1, 0d);
        var scrollWithoutMomentum = Math.Clamp(scrollPos, scrollClampRange.Item1, scrollClampRange.Item2);
        var interpolate = LinearScale((0d, maxScrollPos), (0d, maxThumbPos ?? 0));
        return interpolate(scrollWithoutMomentum);
    }

    /// <summary>
    /// Calculates the ratio of visible content to total content
    /// </summary>
    /// <param name="viewportSize">Visible area size</param>
    /// <param name="contentSize">Total content size</param>
    /// <returns>Ratio between 0 and 1</returns>
    public static double GetThumbRatio(double viewportSize, double contentSize)
    {
        if (contentSize <= 0 || viewportSize <= 0)
        {
            return 0;
        }

        return Math.Clamp(viewportSize / contentSize, 0, 1);
    }

    /// <summary>
    /// Calculates the thumb size for a scrollbar
    /// </summary>
    /// <param name="size">Scroll size parameters</param>
    /// <returns>Calculated thumb size with minimum of 18px</returns>
    public static double GetThumbSize(ScrollSize size)
    {
        if (size.Scrollbar == null)
        {
            return 18;
        }

        var ratio = GetThumbRatio(size.ViewPort, size.Content);
        var scrollbarPadding = size.Scrollbar.PaddingStart + size.Scrollbar.PaddingEnd;
        var availableSize = size.Scrollbar.Size - scrollbarPadding;
        return Math.Max(availableSize * ratio, 18);
    }

    public static Func<double, double> LinearScale((double Min, double Max) input, (double Min, double Max) output)
    {
        if (AreEqual(input.Min, input.Max) || AreEqual(output.Min, output.Max))
        {
            return _ => output.Min;
        }

        double ratio = (output.Max - output.Min) / (input.Max - input.Min);
        return value => output.Min + ratio * (value - input.Min);
    }

    static bool AreEqual(double a, double b)
    {
        if (a == b)
        {
            return true;
        }

        // Handle cases where one or both are zero
        double absA = Math.Abs(a);
        double absB = Math.Abs(b);
        double diff = Math.Abs(a - b);

        if (a == 0 || b == 0 || (absA + absB < double.MinValue))
        {
            return diff < (EPSILON * double.MinValue);
        }

        return diff / Math.Min(absA + absB, double.MaxValue) < EPSILON;
    }

    public class StateMachine<TState, TEvent> where TState : notnull where TEvent : notnull
    {
        TState _currentState;
        readonly Dictionary<TState, Dictionary<TEvent, TState>> _transitions;
        public event Action<TState>? StateChanged;
        public StateMachine(TState initialState, Dictionary<TState, Dictionary<TEvent, TState>> transitions)
        {
            _currentState = initialState;
            _transitions = transitions;
        }
        public TState CurrentState
        {
            get => _currentState;
            private set
            {
                if (!EqualityComparer<TState>.Default.Equals(_currentState, value))
                {
                    _currentState = value;
                    StateChanged?.Invoke(value);
                }
            }
        }

        public void Dispatch(TEvent @event)
        {
            if (_transitions.TryGetValue(_currentState, out var stateTransitions) &&
                stateTransitions.TryGetValue(@event, out var newState))
            {
                _currentState = newState;
            }
        }
    }
}

public enum ScrollState
{
    Hidden,
    Scrolling,
    Interacting,
    Idle
}

public enum ScrollEvent
{
    Scroll,
    ScrollEnd,
    PointerEnter,
    PointerLeave,
    Hide
}
