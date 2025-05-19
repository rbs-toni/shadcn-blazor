using System.Text;
public readonly struct StyleBuilderV2
{
    readonly List<string> _styles;
    readonly string? _userStyles;

    public StyleBuilderV2()
    {
        _styles = new List<string>();
        _userStyles = null;
    }
    public StyleBuilderV2(string? userStyles)
    {
        _styles = new List<string>();
        _userStyles = string.IsNullOrWhiteSpace(userStyles)
            ? null
            : string.Join(";", userStyles.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
    }

    public StyleBuilderV2 AddStyle(string? style) => AddRaw(style);
    public StyleBuilderV2 AddStyle(string prop, string? value) => AddRaw($"{prop}:{value}");
    public StyleBuilderV2 AddStyle(string prop, string? value, bool when = true) => when ? AddStyle(prop, value) : this;
    public StyleBuilderV2 AddStyle(string prop, string? value, Func<bool> when) =>
        AddStyle(prop, value, when != null && when());
    public StyleBuilderV2 AddStyle(string prop, string? value, string? alternativeValue, bool when = true) =>
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
    StyleBuilderV2 AddRaw(string? style)
    {
        if (!string.IsNullOrWhiteSpace(style))
        {
            _styles.Add(style.Trim().TrimEnd(';'));
        }

        return this;
    }
}
