using System;

namespace ShadcnBlazor.Benchmarks.Builders;
public readonly struct CssBuilderV1
{
    readonly HashSet<string> _classes;
    readonly string[]? _userClasses;

    /// <summary>
    /// Initializes a new instance of the <see cref="CssBuilderV1"/> class.
    /// </summary>
    public CssBuilderV1()
    {
        _classes = [];
        _userClasses = null;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CssBuilderV1"/> class.
    /// </summary>
    /// <param name="userClasses">The user classes to include at the end.</param>
    public CssBuilderV1(string? userClasses)
    {
        _classes = [];
        _userClasses = string.IsNullOrWhiteSpace(userClasses)
                     ? null
                     : userClasses.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    }

    /// <summary>
    /// Adds one or more CSS Classes to the builder with space separator.
    /// </summary>
    /// <param name="values">Space-separated CSS Classes to add</param>
    /// <returns>CssBuilder</returns>
    public CssBuilderV1 AddClass(string? values)
    {
        if (!string.IsNullOrWhiteSpace(values))
        {
            _classes.UnionWith(values.Split(' ', StringSplitOptions.RemoveEmptyEntries));
        }
        return this;
    }

    /// <summary>
    /// Adds one or more CSS Classes to the builder with space separator, based on a condition.
    /// </summary>
    /// <param name="value">Space-separated CSS Classes to add</param>
    /// <param name="when">Condition in which the CSS Classes are added.</param>
    /// <returns>CssBuilder</returns>
    public CssBuilderV1 AddClass(string? value, bool when) => when ? AddClass(value) : this;
    public CssBuilderV1 AddClass(string? value, string? alternative, bool when) => when ? AddClass(value) : AddClass(alternative);

    /// <summary>
    /// Adds one or more CSS Classes to the builder with space separator, based on a condition.
    /// </summary>
    /// <param name="value">Space-separated CSS Classes to add</param>
    /// <param name="when">Function that returns a condition in which the CSS Classes are added.</param>
    /// <returns>CssBuilder</returns>
    public CssBuilderV1 AddClass(string? value, Func<bool> when) => when() ? AddClass(value) : this;

    /// <summary>
    /// Finalize the completed CSS Classes as a string.
    /// </summary>
    /// <returns>string</returns>
    public string? Build()
    {
        var allClasses = _userClasses == null
            ? _classes
            : _classes.Union(_userClasses);

        var result = string.Join(" ", allClasses);
        return string.IsNullOrWhiteSpace(result) ? null : result;
    }

    /// <summary>
    /// ToString should only and always call Build to finalize the rendered string.
    /// </summary>
    /// <returns>string</returns>
    public override string? ToString() => Build();
}
