using System;
using System.Linq;

namespace ShadcnBlazor;
public partial class Accordion
{
    readonly InternalAccordionContext _context;

    public Accordion()
    {
        Id = Identifier.NewId();
        _context = new(this);
    }

    string? ClassValue => new CssBuilder(Class)
        .AddClass("w-full")
        .Build();

    public async Task OnKeyDownHandlerAsync(KeyCodeEventArgs args)
    {
        if (args.TargetId != Id || !_context.Items.Any())
        {
            return;
        }

        var targetItem = args.Key switch
        {
            KeyCode.Up => GetPreviousItem(),
            KeyCode.Down => GetNextItem(),
            KeyCode.Home => GetFirstEnabledItem(),
            KeyCode.End => GetLastEnabledItem(),
            _ => null
        };

        if (targetItem != null)
        {
            await targetItem.FocusAsync();
            _context.SetLastActivated(targetItem);
        }
    }
    AccordionItem? FindEnabledItem(int startIndex, bool forward)
    {
        var increment = forward ? 1 : -1;
        for (int i = startIndex; i >= 0 && i < _context.Items.Count; i += increment)
        {
            if (!_context.Items[i].Disabled)
            {
                return _context.Items[i];
            }
        }

        var wrapStart = forward ? 0 : _context.Items.Count - 1;
        var wrapEnd = forward ? startIndex - 1 : startIndex + 1;
        for (int i = wrapStart; forward ? i <= wrapEnd : i >= wrapEnd; i += increment)
        {
            if (!_context.Items[i].Disabled)
            {
                return _context.Items[i];
            }
        }

        return _context.LastActivated;
    }
    AccordionItem? GetFirstEnabledItem()
    {
        return _context.Items.FirstOrDefault(x => !x.Disabled) ?? _context.Items[0];
    }
    AccordionItem? GetLastEnabledItem()
    {
        return _context.Items.LastOrDefault(x => !x.Disabled) ?? _context.Items[^1];
    }
    AccordionItem? GetNextItem()
    {
        if (_context.LastActivated == null)
        {
            return GetFirstEnabledItem();
        }

        var currentIndex = _context.Items.IndexOf(_context.LastActivated);
        var nextIndex = currentIndex >= _context.Items.Count - 1 ? 0 : currentIndex + 1;
        return FindEnabledItem(nextIndex, forward: true);
    }
    AccordionItem? GetPreviousItem()
    {
        if (_context.LastActivated == null)
        {
            return GetFirstEnabledItem();
        }

        var currentIndex = _context.Items.IndexOf(_context.LastActivated);
        var prevIndex = currentIndex <= 0 ? _context.Items.Count - 1 : currentIndex - 1;
        return FindEnabledItem(prevIndex, forward: false);
    }
}
