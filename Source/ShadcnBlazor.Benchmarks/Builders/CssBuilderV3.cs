
namespace ShadcnBlazor.Benchmarks.Builders;

using System;
using System.Collections.Generic;
using System.Text;

public readonly struct CssBuilderV3
{
    readonly List<string> _classes;
    readonly string[]? _userClasses;

    public CssBuilderV3()
    {
        _classes = new();
        _userClasses = null;
    }

    public CssBuilderV3(string? userClasses)
    {
        _classes = new();
        _userClasses = string.IsNullOrWhiteSpace(userClasses)
            ? null
            : userClasses.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }

    public CssBuilderV3 AddClass(string? values)
    {
        if (string.IsNullOrWhiteSpace(values))
        {
            return this;
        }

        var span = values.AsSpan().Trim();
        int start = 0;

        for (int i = 0; i <= span.Length; i++)
        {
            if (i == span.Length || span[i] == ' ')
            {
                if (i > start)
                {
                    var cls = span.Slice(start, i - start).ToString();
                    if (!_classes.Contains(cls))
                    {
                        _classes.Add(cls);
                    }
                }
                start = i + 1;
            }
        }

        return this;
    }

    public CssBuilderV3 AddClass(string? value, bool when) =>
        when ? AddClass(value) : this;

    public CssBuilderV3 AddClass(string? value, string? alternative, bool when) =>
        when ? AddClass(value) : AddClass(alternative);

    public CssBuilderV3 AddClass(string? value, Func<bool> when) =>
        when() ? AddClass(value) : this;

    public string? Build()
    {
        if (_classes.Count == 0 && _userClasses == null)
        {
            return null;
        }

        var sb = new StringBuilder();

        foreach (var cls in _classes)
        {
            if (sb.Length > 0)
            {
                sb.Append(' ');
            }

            sb.Append(cls);
        }

        if (_userClasses != null)
        {
            foreach (var cls in _userClasses)
            {
                if (string.IsNullOrWhiteSpace(cls))
                {
                    continue;
                }

                if (sb.Length > 0)
                {
                    sb.Append(' ');
                }

                sb.Append(cls);
            }
        }

        return sb.Length == 0 ? null : sb.ToString();
    }

    public override string? ToString() => Build();
}


