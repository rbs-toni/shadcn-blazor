namespace ShadcnBlazor.Benchmarks.Builders;
public readonly struct StyleBuilderV1
{
    readonly HashSet<string> _styles;
    readonly string? _userStyles;

    /// <summary>
    /// Initializes a new instance of the <see cref="StyleBuilderV1"/> class.
    /// </summary>
    public StyleBuilderV1()
    {
        _styles = [];
        _userStyles = null;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StyleBuilderV1"/> class.
    /// </summary>
    /// <param name="userStyles">The user styles to include at the end.</param>
    public StyleBuilderV1(string? userStyles)
    {
        _styles = [];
        _userStyles = string.IsNullOrWhiteSpace(userStyles)
                    ? null
                    : string.Join(";", userStyles.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                                                  .Where(i => !string.IsNullOrWhiteSpace(i)));
    }

    /// <summary>
    /// Adds a conditional in-line style to the builder with space separator and closing semicolon.
    /// </summary>
    /// <param name="style"></param>
    public StyleBuilderV1 AddStyle(string? style) => AddRaw(style);

    /// <summary>
    /// Adds a conditional in-line style to the builder with space separator and closing semicolon..
    /// </summary>
    /// <param name="prop"></param>
    /// <param name="value">Style to add</param>
    /// <returns>StyleBuilder</returns>
    public StyleBuilderV1 AddStyle(string prop, string? value) => AddRaw($"{prop}:{value}");

    /// <summary>
    /// Adds a conditional in-line style to the builder with space separator and closing semicolon..
    /// </summary>
    /// <param name="prop"></param>
    /// <param name="value">Style to conditionally add.</param>
    /// <param name="when">Condition in which the style is added.</param>
    /// <returns>StyleBuilder</returns>
    public StyleBuilderV1 AddStyle(string prop, string? value, bool when = true) => when ? AddStyle(prop, value) : this;
    public StyleBuilderV1 AddStyle(string prop, string? value, string? alternativeValue, bool when = true) => when ? AddStyle(prop, value) : AddStyle(prop, alternativeValue);

    /// <summary>
    /// Adds a conditional in-line style to the builder with space separator and closing semicolon..
    /// </summary>
    /// <param name="prop"></param>
    /// <param name="value">Style to conditionally add.</param>
    /// <param name="when">Condition in which the style is added.</param>
    /// <returns>StyleBuilder</returns>
    public StyleBuilderV1 AddStyle(string prop, string? value, Func<bool> when) => AddStyle(prop, value, when != null && when());

    /// <summary>
    /// Finalize the completed Style as a string.
    /// </summary>
    /// <returns>string</returns>
    public string? Build()
    {
        var allStyles = string.IsNullOrWhiteSpace(_userStyles)
                      ? _styles
                      : _styles.Union([_userStyles]);

        if (!allStyles.Any())
        {
            return null;
        }

        return string.Concat(allStyles.Select(s => $"{s};")).TrimEnd().TrimEnd(';');
    }

    /// <summary>
    /// ToString should only and always call Build to finalize the rendered string.
    /// </summary>
    /// <returns></returns>
    public override string? ToString() => Build();

    /// <summary>
    /// Adds a raw string to the builder that will be concatenated with the next style or value added to the builder.
    /// </summary>
    /// <param name="style"></param>
    /// <returns>StyleBuilder</returns>
    StyleBuilderV1 AddRaw(string? style)
    {
        if (!string.IsNullOrWhiteSpace(style))
        {
            _styles.Add(style.Trim().TrimEnd(';'));
        }

        return this;
    }
}
