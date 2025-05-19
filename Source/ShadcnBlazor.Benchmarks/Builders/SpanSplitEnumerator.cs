namespace ShadcnBlazor.Benchmarks.Builders;
// Optimized span splitter (replaces string.Split allocations)
ref struct SpanSplitEnumerator
{
    ReadOnlySpan<char> _remaining;
    readonly char _separator;
    bool _isDone;

    public SpanSplitEnumerator(ReadOnlySpan<char> buffer, char separator)
    {
        _remaining = buffer;
        _separator = separator;
        Current = default;
        _isDone = false;
    }

    public ReadOnlySpan<char> Current { get; private set; }

    public SpanSplitEnumerator GetEnumerator() => this;
    public bool MoveNext()
    {
        if (_isDone)
        {
            return false;
        }

        int idx = _remaining.IndexOf(_separator);
        if (idx < 0)
        {
            Current = _remaining;
            _remaining = default;
            _isDone = true;
        }
        else
        {
            Current = _remaining.Slice(0, idx);
            _remaining = _remaining.Slice(idx + 1);
        }
        return true;
    }
}

