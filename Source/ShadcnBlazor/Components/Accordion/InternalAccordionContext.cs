using Microsoft.AspNetCore.Components;
using System;
using System.Linq;

namespace ShadcnBlazor;
class InternalAccordionContext
{
    readonly Accordion _accordion;
    readonly HashSet<AccordionItem> _activeItems = [];
    readonly List<AccordionItem> _items = [];
    AccordionItem? _lastActivated;

    public InternalAccordionContext(Accordion accordion)
    {
        _accordion = accordion;
    }

    public event Action? OnStateChanged;

    public bool Collapsible => _accordion.Collapsible;
    public Direction Direction => _accordion.Direction;
    public bool Disabled => _accordion.Disabled;
    public bool IsSingle => _accordion.Type == SingleOrMultiple.Single;
    public List<AccordionItem> Items => _items;
    public AccordionItem? LastActivated => _lastActivated;
    public Orientation Orientation => _accordion.Orientation;
    public ElementReference ParentElement { get; set; }

    public bool IsActive(AccordionItem item)
    {
        if (item == null)
        {
            return false;
        }

        return _activeItems.Contains(item);
    }
    public void RegisterItem(AccordionItem item)
    {
        if (item != null && !_items.Contains(item))
        {
            _items.Add(item);
        }

        if (!Collapsible && _items.Any())
        {
            // Set the first item as active if no items are currently active
            if (!_activeItems.Any())
            {
                _activeItems.Add(_items.First());
                _lastActivated = _items.First();
                NotifyStateHasChanged();
            }
        }
    }
    public void SetActiveItem(AccordionItem item)
    {
        if (Disabled)
        {
            return;
        }

        bool isActive = _activeItems.Contains(item);

        if (!IsSingle)
        {
            if (isActive)
            {
                if (Collapsible)
                {
                    // Prevent collapsing the last item if collapsible is false (even though logically shouldn't happen here)
                    if (!Collapsible && _activeItems.Count == 1)
                    {
                        return;
                    }

                    _activeItems.Remove(item);
                }
            }
            else
            {
                _activeItems.Add(item);
            }
        }
        else
        {
            if (isActive)
            {
                if (Collapsible)
                {
                    _activeItems.Clear();
                    _lastActivated = item;
                }
                // else: do nothing (must keep one open)
            }
            else
            {
                _activeItems.Clear();
                _activeItems.Add(item);
                _lastActivated = item;
            }
        }
        NotifyStateHasChanged();
    }
    public void SetLastActivated(AccordionItem? lastActivated)
    {
        _lastActivated = lastActivated;
    }
    public void UnregisterItem(AccordionItem item)
    {
        if (item != null && _items.Contains(item))
        {
            _items.Remove(item);
            _activeItems.Remove(item);
            if (_lastActivated == item)
            {
                _lastActivated = null;
            }
        }
    }
    void NotifyStateHasChanged()
    {
        if (OnStateChanged != null)
        {
            OnStateChanged.Invoke();
        }
    }
}
