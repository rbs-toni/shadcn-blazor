using System.Reflection;

namespace ShadcnBlazor;
public abstract record Enumeration<T> : IComparable<T> where T : Enumeration<T>
{
    static readonly Lazy<Dictionary<int, T>> AllItems;
    static readonly Lazy<Dictionary<string, T>> AllItemsByName;

    static Enumeration()
    {
        AllItems = new Lazy<Dictionary<int, T>>(() => typeof(T)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .Where(x => x.FieldType == typeof(T))
                .Select(x => x.GetValue(null))
                .Cast<T>()
                .ToDictionary(x => x.Value, x => x));

        AllItemsByName = new Lazy<Dictionary<string, T>>(() =>
        {
            var items = new Dictionary<string, T>(AllItems.Value.Count);
            foreach (var item in AllItems.Value)
            {
                if (!items.TryAdd(item.Value.DisplayName, item.Value))
                {
                    throw new Exception(
                        $"DisplayName needs to be unique. '{item.Value.DisplayName}' already exists");
                }
            }
            return items;
        });
    }
    protected Enumeration(int value, string displayName)
    {
        Value = value;
        DisplayName = displayName;
    }

    public string DisplayName { get; }
    public int Value { get; }

    public static int AbsoluteDifference(Enumeration<T> firstValue, Enumeration<T> secondValue)
    {
        return Math.Abs(firstValue.Value - secondValue.Value);
    }
    public static T FromDisplayName(string displayName)
    {
        if (AllItemsByName.Value.TryGetValue(displayName, out var matchingItem))
        {
            return matchingItem;
        }
        throw new InvalidOperationException($"'{displayName}' is not a valid display name in {typeof(T)}");
    }
    public static T FromValue(int value)
    {
        if (AllItems.Value.TryGetValue(value, out var matchingItem))
        {
            return matchingItem;
        }
        throw new InvalidOperationException($"'{value}' is not a valid value in {typeof(T)}");
    }
    public static IEnumerable<T> GetAll()
    {
        return AllItems.Value.Values;
    }

    public int CompareTo(T? other) => Value.CompareTo(other!.Value);
    public override string ToString() => DisplayName;
}