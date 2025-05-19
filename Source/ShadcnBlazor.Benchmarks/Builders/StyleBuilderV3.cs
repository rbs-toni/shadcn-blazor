using System;
using System.Runtime.CompilerServices;

namespace ShadcnBlazor.Benchmarks.Builders;
public readonly struct StyleBuilderV3
{
    readonly HashSet<string> _styles;
    readonly string? _userStyles;

    public StyleBuilderV3()
    {
        _styles = new HashSet<string>(StringComparer.Ordinal);
        _userStyles = null;
    }
    public StyleBuilderV3(string? userStyles)
    {
        _styles = new HashSet<string>(StringComparer.Ordinal);
        _userStyles = string.IsNullOrWhiteSpace(userStyles) ? null : userStyles;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public StyleBuilderV3 AddStyle(string? style) => AddRaw(style);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public StyleBuilderV3 AddStyle(string prop, string? value)
    {
        if (!string.IsNullOrWhiteSpace(prop) && value != null)
        {
            _styles.Add($"{prop}:{value}");
        }
        return this;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public StyleBuilderV3 AddStyle(string prop, string? value, bool when) => when ? AddStyle(prop, value) : this;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public StyleBuilderV3 AddStyle(string prop, string? value, Func<bool> when)
        => when != null ? AddStyle(prop, value, when()) : this;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public StyleBuilderV3 AddStyle(string prop, string? value, string? alternativeValue, bool when)
        => when ? AddStyle(prop, value) : AddStyle(prop, alternativeValue);
    public string? Build()
    {
        if (_styles.Count == 0)
        {
            return _userStyles;
        }

        using var sb = new ValueStringBuilder(stackalloc char[256]);

        // Build the main styles
        bool first = true;
        foreach (var style in _styles)
        {
            if (!first)
            {
                sb.Append(';');
            }
            sb.Append(style);
            first = false;
        }

        // Append user styles if they exist
        if (!string.IsNullOrEmpty(_userStyles))
        {
            if (!first)
            {
                sb.Append(';');
            }
            sb.Append(_userStyles);
        }

        return sb.Length > 0 ? sb.ToString() : null;
    }
    public override string? ToString() => Build();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    StyleBuilderV3 AddRaw(string? style)
    {
        if (!string.IsNullOrWhiteSpace(style))
        {
            var span = style.AsSpan();
            int start = 0;
            for (int i = 0; i <= span.Length; i++)
            {
                if (i == span.Length || span[i] == ';')
                {
                    var segment = span.Slice(start, i - start).Trim();
                    if (!segment.IsEmpty)
                    {
                        _styles.Add(segment.ToString());
                    }
                    start = i + 1;
                }
            }
        }
        return this;
    }
}
