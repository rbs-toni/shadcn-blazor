using System;
using System.Linq;

namespace ShadcnBlazor;
class InternalAccordionItemContext
{
    readonly AccordionItem _accordionItem;

    public InternalAccordionItemContext(AccordionItem accordionItem)
    {
        _accordionItem = accordionItem;
        TriggerId = Identifier.NewId();
    }

    public string DataState => _accordionItem.Open ? "open" : "closed";
    public bool Disabled => _accordionItem.IsDisabled;
    public bool Open => _accordionItem.Open;
    public string TriggerId { get; }

    public void SetActiveItem(InternalAccordionContext accordion)
    {
        bool triggerDisabled = accordion.IsSingle && _accordionItem.Open && !accordion.Collapsible;
        if (_accordionItem.Disabled || triggerDisabled)
        {
            return;
        }
        accordion.SetActiveItem(_accordionItem);
    }

    public async Task FocusAsync()
    {
        await OnFocus.Invoke();
    }

    public event Func<Task> OnFocus;
}
