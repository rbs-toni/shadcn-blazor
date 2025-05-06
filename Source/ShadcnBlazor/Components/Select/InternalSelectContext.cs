using Microsoft.AspNetCore.Components;
using System;
using System.Linq;

namespace ShadcnBlazor;
public class InternalSelectContext<TItem> where TItem : notnull
{
    readonly ICollection<SelectItem<TItem>> _items = [];

    public InternalSelectContext(Select<TItem> selectComponent)
    {
        SelectComponent = selectComponent;
    }

    public ICollection<SelectItem<TItem>> Items => _items;
    public Select<TItem> SelectComponent { get; }
    /// <summary>
    /// Gets the event callback to be invoked when the selected value is changed.
    /// </summary>
    public EventCallback<TItem?> SelectedItemChanged { get; set; }
    /// <summary>
    /// Gets the event callback to be invoked when the selected value is changed.
    /// </summary>
    public EventCallback<string?> ValueChanged { get; set; }

    internal void Register(SelectItem<TItem> item)
    {
        if (item is null)
        {
            return;
        }

        if (!_items.Contains(item))
        {
            _items.Add(item);
        }
    }
    internal void Unregister(SelectItem<TItem> item)
    {
        if (item is null)
        {
            return;
        }

        _items.Remove(item);
    }
}
