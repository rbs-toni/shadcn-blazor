using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace ShadcnBlazor;

internal ref struct ValueStringBuilder
{
    private char[]? _arrayToReturnToPool;
    private Span<char> _chars;
    private int _pos;

    public ValueStringBuilder(Span<char> initialBuffer)
    {
        _arrayToReturnToPool = null;
        _chars = initialBuffer;
        _pos = 0;
    }

    public void Append(ReadOnlySpan<char> value)
    {
        if (value.Length == 0) return;

        if (value.Length > _chars.Length - _pos)
        {
            Grow(value.Length);
        }
        value.CopyTo(_chars.Slice(_pos));
        _pos += value.Length;
    }

    public void Append(char c)
    {
        if (_pos >= _chars.Length)
        {
            Grow(1);
        }
        _chars[_pos++] = c;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void Grow(int additionalCapacityBeyondPos)
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

    public void Dispose()
    {
        char[]? toReturn = _arrayToReturnToPool;
        this = default;
        if (toReturn != null)
        {
            ArrayPool<char>.Shared.Return(toReturn);
        }
    }

    public override string ToString() => _chars.Slice(0, _pos).ToString();

    public int Length => _pos;
}
