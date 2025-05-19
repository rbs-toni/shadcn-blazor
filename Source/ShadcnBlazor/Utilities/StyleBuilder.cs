using System.Text;
using System.Timers;

namespace ShadcnBlazor;
public readonly struct StyleBuilder
{
    readonly List<string> _styles;
    readonly string? _userStyles;

    public StyleBuilder()
    {
        _styles = new List<string>();
        _userStyles = null;
    }
    public StyleBuilder(string? userStyles)
    {
        _styles = new List<string>();
        _userStyles = string.IsNullOrWhiteSpace(userStyles)
            ? null
            : string.Join(";", userStyles.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
    }

    public StyleBuilder AddStyle(string? style) => AddRaw(style);
    public StyleBuilder AddStyle(string prop, string? value) => AddRaw($"{prop}:{value}");
    public StyleBuilder AddStyle(string prop, string? value, bool when = true) => when ? AddStyle(prop, value) : this;
    public StyleBuilder AddStyle(string prop, string? value, Func<bool> when) =>
        AddStyle(prop, value, when != null && when());
    public StyleBuilder AddStyle(string prop, string? value, string? alternativeValue, bool when = true) =>
        AddStyle(prop, when ? value : alternativeValue);
    public string? Build()
    {
        if (_styles.Count == 0 && string.IsNullOrWhiteSpace(_userStyles))
        {
            return null;
        }

        var builder = new StringBuilder();

        foreach (var style in _styles)
        {
            builder.Append(style);
            builder.Append(";");
        }

        if (!string.IsNullOrWhiteSpace(_userStyles))
        {
            builder.Append(_userStyles);
            builder.Append(";");
        }

        return builder.ToString().TrimEnd().TrimEnd(';');
    }
    public override string? ToString() => Build();
    StyleBuilder AddRaw(string? style)
    {
        if (!string.IsNullOrWhiteSpace(style))
        {
            _styles.Add(style.Trim().TrimEnd(';'));
        }

        return this;
    }
}
