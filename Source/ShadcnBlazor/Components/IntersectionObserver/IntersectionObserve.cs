using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Linq;

namespace ShadcnBlazor;
public class IntersectionObserve : ComponentBase, IAsyncDisposable
{
    const string NO_ELEMENT_MESSAGE = "The element reference to observe is required, for example: @ref=\"Context.Ref.Current\" must be provided on the element";

    [Parameter]
    public RenderFragment<IntersectionObserverContext>? ChildContent { get; set; }
    public IntersectionObserverContext IntersectionObserverContext { get; set; } = new IntersectionObserverContext();
    [Parameter] 
    public bool IsIntersecting { get; set; }
    [Parameter] 
    public EventCallback<bool> IsIntersectingChanged { get; set; }
    [Parameter] 
    public bool Once { get; set; }
    [Parameter] 
    public EventCallback<IntersectionObserverEntry> OnChange { get; set; }
    [Parameter] 
    public EventCallback OnDisposed { get; set; }
    [Parameter] 
    public IntersectionObserverOptions? Options { get; set; }
    IntersectionObserver? Observer { get; set; }
    [Inject] 
    IIntersectionObserverService? ObserverService { get; set; }

    public async ValueTask DisposeAsync()
    {
        var observer = Observer;

        if (observer == null)
        {
            return;
        }

        Observer = null;
        await observer.Dispose();
        await OnDisposed.InvokeAsync();
    }
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (ChildContent == null)
        {
            throw new Exception($"No element found to observe. {NO_ELEMENT_MESSAGE}");
        }
        ChildContent(IntersectionObserverContext)(builder);
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await InitialiseObserver();
        }
    }
    async Task InitialiseObserver()
    {
        var elementReference = IntersectionObserverContext?.Ref?.Current;

        if (elementReference == null || Equals(elementReference, default(ElementReference)))
        {
            throw new Exception(NO_ELEMENT_MESSAGE);
        }
        if (ObserverService != null)
        {
            Observer = await ObserverService.ObserveAsync(elementReference.Value, OnIntersectUpdate, Options);
        }
    }
    async Task OnIntersectUpdate(IList<IntersectionObserverEntry> entries)
    {
        var entry = entries?.FirstOrDefault();

        if (entry == null)
        {
            return;
        }

        await IsIntersectingChanged.InvokeAsync(entry.IsIntersecting);
        await OnChange.InvokeAsync(entry);

        IntersectionObserverContext.Entry = entry;
        StateHasChanged();

        if (Once && entry.IsIntersecting)
        {
            if (Observer != null)
            {
                await Observer.Dispose();
                Observer = null;
            }
        }
    }
}
