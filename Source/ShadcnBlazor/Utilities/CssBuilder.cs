using System.Buffers;
using System.Runtime.CompilerServices;

namespace ShadcnBlazor;
public readonly struct CssBuilder
{
    readonly HashSet<string> _classes;
    readonly string? _userClasses;

    public CssBuilder()
    {
        _classes = new HashSet<string>(StringComparer.Ordinal);
        _userClasses = null;
    }
    public CssBuilder(string? userClasses)
    {
        _classes = new HashSet<string>(StringComparer.Ordinal);
        _userClasses = string.IsNullOrWhiteSpace(userClasses) ? null : userClasses;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CssBuilder AddClass(string? values)
    {
        if (!string.IsNullOrWhiteSpace(values))
        {
            foreach (var cls in new SpanSplitEnumerator(values.AsSpan(), ' '))
            {
                if (!cls.IsEmpty)
                {
                    _classes.Add(cls.ToString());
                }
            }
        }
        return this;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CssBuilder AddClass(string? value, bool when) => when ? AddClass(value) : this;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CssBuilder AddClass(string? value, Func<bool> when) => AddClass(value, when());
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CssBuilder AddClass(string? value, string? alternative, bool when) => when ? AddClass(value) : AddClass(alternative);
    public string? Build()
    {
        if (_classes.Count == 0)
        {
            return _userClasses;
        }

        if (string.IsNullOrEmpty(_userClasses))
        {
            return string.Join(' ', _classes);
        }

        using var sb = new ValueStringBuilder(stackalloc char[256]);
        foreach (var cls in _classes)
        {
            sb.Append(cls);
            sb.Append(' ');
        }
        sb.Append(_userClasses);
        return sb.ToString();
    }
    public override string? ToString() => Build();
}
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
// Add this to your project
ref struct ValueStringBuilder
{
    char[]? _arrayToReturnToPool;
    Span<char> _chars;
    int _pos;

    public ValueStringBuilder(Span<char> initialBuffer)
    {
        _arrayToReturnToPool = null;
        _chars = initialBuffer;
        _pos = 0;
    }

    public int Length => _pos;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(char c)
    {
        int pos = _pos;
        if ((uint)pos < (uint)_chars.Length)
        {
            _chars[pos] = c;
            _pos = pos + 1;
        }
        else
        {
            GrowAndAppend(c);
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(string? s)
    {
        if (s == null)
        {
            return;
        }

        int pos = _pos;
        if (s.Length == 1 && (uint)pos < (uint)_chars.Length)
        {
            _chars[pos] = s[0];
            _pos = pos + 1;
        }
        else
        {
            AppendSlow(s);
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(ReadOnlySpan<char> value)
    {
        int pos = _pos;
        if (pos > _chars.Length - value.Length)
        {
            Grow(value.Length);
        }

        value.CopyTo(_chars.Slice(_pos));
        _pos += value.Length;
    }
    public void Dispose()
    {
        char[]? toReturn = _arrayToReturnToPool;
        this = default; // Reset for safety
        if (toReturn != null)
        {
            ArrayPool<char>.Shared.Return(toReturn);
        }
    }
    public override string ToString()
    {
        return _chars.Slice(0, _pos).ToString();
    }
    void AppendSlow(string s)
    {
        int pos = _pos;
        if (pos > _chars.Length - s.Length)
        {
            Grow(s.Length);
        }

        s.AsSpan().CopyTo(_chars.Slice(pos));
        _pos += s.Length;
    }
    [MethodImpl(MethodImplOptions.NoInlining)]
    void Grow(int additionalCapacityBeyondPos)
    {
        char[] poolArray = ArrayPool<char>.Shared.Rent(Math.Max(_pos + additionalCapacityBeyondPos, _chars.Length * 2));

        _chars.Slice(0, _pos).CopyTo(poolArray);

        char[]? toReturn = _arrayToReturnToPool;
        _chars = _arrayToReturnToPool = poolArray;

        if (toReturn != null)
        {
            ArrayPool<char>.Shared.Return(toReturn);
        }
    }
    void GrowAndAppend(char c)
    {
        Grow(1);
        Append(c);
    }
}
