using Microsoft.AspNetCore.Components;
using System;
using System.Linq;

namespace ShadcnBlazor;
public class IntersectionObserver
{
    /// <summary>
    /// Initialise the intersection observer with the
    /// actions that will be triggered.
    /// </summary>
    /// <param name="id">The observer id</param>
    /// <param name="onIntersectUpdate">On intersection updates</param>
    /// <param name="onUnobserve">Unobserving an element</param>
    /// <param name="onDisconnect">On disconnecting the observer</param>
    public IntersectionObserver(
        string id,
        Action<IList<IntersectionObserverEntry>> onIntersectUpdate,
        Func<string, ElementReference, ValueTask> onObserve,
        Func<string, ElementReference, ValueTask> onUnobserve,
        Func<string, ValueTask<bool>> onDisconnect,
        Func<string, ValueTask> onRemove
    )
    {
        Id = id;
        OnIntersectUpdate = onIntersectUpdate;
        OnObserve = onObserve;
        OnUnobserve = onUnobserve;
        OnDisconnect = onDisconnect;
        OnRemove = onRemove;
    }

    public IntersectionObserver(
    string id,
    Func<IList<IntersectionObserverEntry>, Task> onIntersectUpdate,
    Func<string, ElementReference, ValueTask> onObserve,
    Func<string, ElementReference, ValueTask> onUnobserve,
    Func<string, ValueTask<bool>> onDisconnect,
    Func<string, ValueTask> onRemove
)
    {
        Id = id;
        OnIntersectUpdateFunc = onIntersectUpdate;
        OnObserve = onObserve;
        OnUnobserve = onUnobserve;
        OnDisconnect = onDisconnect;
        OnRemove = onRemove;
    }

    /// <summary>
    /// On disconnecting the observer, trigger the action
    /// </summary>
    event Func<string, ValueTask<bool>> OnDisconnect;
    /// <summary>
    /// On intersection updates, trigger the action
    /// </summary>
    event Action<IList<IntersectionObserverEntry>>? OnIntersectUpdate;
    event Func<IList<IntersectionObserverEntry>, Task>? OnIntersectUpdateFunc;
    /// <summary>
    /// On unobserving an element, trigger the action
    /// </summary>
    event Func<string, ElementReference, ValueTask> OnObserve;
    /// <summary>
    /// On disconnecting the observer, trigger the action
    /// </summary>
    event Func<string, ValueTask> OnRemove;
    /// <summary>
    /// On unobserving an element, trigger the action
    /// </summary>
    event Func<string, ElementReference, ValueTask> OnUnobserve;

    /// <summary>
    /// The identifier for the observer instance
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// On disconnecting the observer, trigger
    /// the action(s).
    /// </summary>
    public async ValueTask Disconnect()
    {
        await OnDisconnect.Invoke(Id);
    }
    /// <summary>
    /// Signal that the observer should be
    /// disposed.
    /// </summary>
    public async ValueTask Dispose()
    {
        await OnRemove.Invoke(Id);
    }
    /// <summary>
    /// On observing an element, pass the element
    /// reference to the action(s).
    /// </summary>
    /// <param name="elementRef">The element to observe</param>
    public async ValueTask Observe(ElementReference elementRef)
    {
        await OnObserve.Invoke(Id, elementRef);
    }
    /// <summary>
    /// On intersection, pass the entries
    /// to the actions(s).
    /// </summary>
    /// <param name="entries"></param>
    public void OnIntersect(IList<IntersectionObserverEntry> entries)
    {
        if (entries != null && entries.Any())
        {
            OnIntersectUpdate?.Invoke(entries);
            OnIntersectUpdateFunc?.Invoke(entries);
        }
    }
    /// <summary>
    /// On unobserving an element, pass the element
    /// reference to the action(s).
    /// </summary>
    /// <param name="elementRef">The element to unobserve</param>
    public async ValueTask Unobserve(ElementReference elementRef)
    {
        await OnUnobserve.Invoke(Id, elementRef);
    }
}
