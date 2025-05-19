using System.Collections;
using System.Linq.Expressions;

namespace ShadcnBlazor;
public class DialogParameters : IEnumerable<KeyValuePair<string, object?>>
{
    /// <summary>
    /// The default dialog parameters.
    /// This field is only intended for parameters that do not differ from their default values.
    /// </summary>
    internal static readonly DialogParameters Default = new();
    internal Dictionary<string, object?> _parameters = new();

    /// <summary>
    /// The number of parameters.
    /// </summary>
    public int Count => _parameters.Count;

    /// <summary>
    /// Gets or sets a parameter.
    /// </summary>
    /// <param name="parameterName">The name of the parameter to find.</param>
    /// <returns>The parameter value.</returns>
    public object? this[string parameterName]
    {
        get => Get<object?>(parameterName);
        set => _parameters[parameterName] = value;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _parameters.GetEnumerator();
    }

    /// <summary>
    /// Adds or updates a parameter.
    /// </summary>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <param name="value">The value to add or update.</param>
    public void Add(string parameterName, object? value)
    {
        _parameters[parameterName] = value;
    }
    /// <summary>
    /// Gets an existing parameter.
    /// </summary>
    /// <typeparam name="T">The type of value to return.</typeparam>
    /// <param name="parameterName">The name of the parameter to find.</param>
    /// <returns>The parameter value, if it exists.</returns>
    public T? Get<T>(string parameterName)
    {
        if (_parameters.TryGetValue(parameterName, out var value))
        {
            return (T?)value;
        }

        throw new KeyNotFoundException($"{parameterName} does not exist in Dialog parameters");
    }
    /// <summary>
    /// Gets an enumerator for all parameters.
    /// </summary>
    /// <returns>An enumerator of <see cref="KeyValuePair{TKey, TValue}"/> values.</returns>
    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
    {
        return _parameters.GetEnumerator();
    }
    /// <summary>
    /// Gets an existing parameter or a default value if nothing was found.
    /// </summary>
    /// <typeparam name="T">The type of value to return.</typeparam>
    /// <param name="parameterName">The name of the parameter to find.</param>
    /// <returns>The parameter value, if it exists.</returns>
    public T? TryGet<T>(string parameterName)
    {
        if (_parameters.TryGetValue(parameterName, out var value))
        {
            return (T?)value;
        }

        return default;
    }
}
public class DialogParameters<T> : DialogParameters
{
    /// <summary>
    /// Adds a parameter using a member expression.
    /// </summary>
    /// <typeparam name="TParam">The type of parameter to add.</typeparam>
    /// <param name="propertyExpression">The property to add as a parameter.</param>
    /// <param name="value">The parameter value.</param>
    public void Add<TParam>(Expression<Func<T, TParam>> propertyExpression, TParam value)
    {
        ArgumentNullException.ThrowIfNull(propertyExpression);
        if (propertyExpression.Body is not MemberExpression memberExpression)
        {
            throw new ArgumentException($"Argument '{nameof(propertyExpression)}' must be a '{nameof(MemberExpression)}'");
        }

        Add(memberExpression.Member.Name, value);
    }
    /// <summary>
    /// Gets a parameter using a property expression.
    /// </summary>
    /// <typeparam name="TParam">The type of parameter to get.</typeparam>
    /// <param name="propertyExpression">The property to get as a parameter.</param>
    /// <returns>The parameter value.</returns>
    public TParam? Get<TParam>(Expression<Func<T, TParam>> propertyExpression)
    {
        ArgumentNullException.ThrowIfNull(propertyExpression);
        if (propertyExpression.Body is not MemberExpression memberExpression)
        {
            throw new ArgumentException($"Argument '{nameof(propertyExpression)}' must be a '{nameof(MemberExpression)}'");
        }

        return Get<TParam?>(memberExpression.Member.Name);
    }
    /// <summary>
    /// Gets a parameter using a property expression or a default value if no parameter was found.
    /// </summary>
    /// <typeparam name="TParam">The type of parameter to get.</typeparam>
    /// <param name="propertyExpression">The property to get as a parameter.</param>
    /// <returns>The parameter value.</returns>
    public TParam? TryGet<TParam>(Expression<Func<T, TParam>> propertyExpression)
    {
        ArgumentNullException.ThrowIfNull(propertyExpression);
        if (propertyExpression.Body is not MemberExpression memberExpression)
        {
            throw new ArgumentException($"Argument '{nameof(propertyExpression)}' must be a '{nameof(MemberExpression)}'");
        }

        return TryGet<TParam>(memberExpression.Member.Name);
    }
}
