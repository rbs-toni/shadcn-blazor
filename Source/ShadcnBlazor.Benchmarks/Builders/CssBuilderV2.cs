using System.Buffers;
using System.Runtime.CompilerServices;

namespace ShadcnBlazor.Benchmarks.Builders;
public readonly struct CssBuilderV2
{
    readonly HashSet<string> _classes;
    readonly string? _userClasses;

    public CssBuilderV2()
    {
        _classes = new HashSet<string>(StringComparer.Ordinal);
        _userClasses = null;
    }
    public CssBuilderV2(string? userClasses)
    {
        _classes = new HashSet<string>(StringComparer.Ordinal);
        _userClasses = string.IsNullOrWhiteSpace(userClasses) ? null : userClasses;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CssBuilderV2 AddClass(string? values)
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
    public CssBuilderV2 AddClass(string? value, bool when) => when ? AddClass(value) : this;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CssBuilderV2 AddClass(string? value, Func<bool> when) => AddClass(value, when());
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CssBuilderV2 AddClass(string? value, string? alternative, bool when) => when ? AddClass(value) : AddClass(alternative);
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

