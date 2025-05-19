using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace ShadcnBlazor;
public class IntersectionObserverService : JSModule, IIntersectionObserverService, IAsyncDisposable
{
    readonly DotNetObjectReference<IntersectionObserverService>? _objectRef;
    readonly ConcurrentDictionary<string, IntersectionObserver> _observers = new ConcurrentDictionary<string, IntersectionObserver>();

    public IntersectionObserverService(IJSRuntime jsRuntime, NavigationManager navManager) : base(jsRuntime, "./_content/ShadcnBlazor/modules/intersection-observer.min.js")
    {
        _objectRef ??= DotNetObjectReference.Create(this);
    }

    /// <summary>
    /// Create an observer instance with a unique id, adding it
    /// to the list of observers.
    /// </summary>
    /// <param name="onIntersectUpdate">On an intersection update, pass the entries to the callback</param>
    /// <param name="options">The intersection observer options</param>
    /// <returns>The observer instance</returns>
    public async Task<IntersectionObserver> CreateAsync(
        Action<IList<IntersectionObserverEntry>> onIntersectUpdate,
        IntersectionObserverOptions? options = null
    )
    {
        var callbackId = Guid.NewGuid().ToString();
        await InvokeAsync<object>("create", _objectRef, callbackId, options);
        return CreateObserver(callbackId, onIntersectUpdate);
    }
    public Task<IntersectionObserver> CreateAsync(Func<IList<IntersectionObserverEntry>, Task> onIntersectUpdate, IntersectionObserverOptions? options = null)
    {
        throw new NotImplementedException();
    }
    /// <summary>
    /// Create an observer instance and immediately observe 
    /// the element.
    /// </summary>
    /// <param name="element">The element</param>
    /// <param name="onIntersectUpdate">On an intersection update, pass the entries</param>
    /// <param name="options">The intersection observer options</param>
    /// <returns></returns>
    public async Task<IntersectionObserver> ObserveAsync(
        ElementReference element,
        Action<IList<IntersectionObserverEntry>> onIntersectUpdate,
        IntersectionObserverOptions? options = null
    )
    {
        var callbackId = Identifier.NewId();
        await InvokeAsync<object>("observe", _objectRef, callbackId, element, options);
        return CreateObserver(callbackId, onIntersectUpdate);
    }
    public async Task<IntersectionObserver> ObserveAsync(ElementReference element, Func<IList<IntersectionObserverEntry>, Task> onIntersectUpdate, IntersectionObserverOptions? options = null)
    {
        var callbackId = Identifier.NewId();
        await InvokeAsync<object>("observe", _objectRef, callbackId, element, options);
        return CreateObserver(callbackId, onIntersectUpdate);
    }
    /// <summary>
    /// This is triggered in js when an intersection update
    /// has occurred.
    /// </summary>
    /// <param name="id">The observer id</param>
    /// <param name="entries">The intersection observer entries</param>
    [JSInvokable(nameof(OnCallback))]
    public void OnCallback(string id, IList<IntersectionObserverEntry> entries)
    {
        EnsureObserverExists(id);

        if (_observers.TryGetValue(id, out var observer))
        {
            observer.OnIntersect(entries);
        }
    }
    public Task OnCallbackAsync(string id, IList<IntersectionObserverEntry> entries)
    {
        throw new NotImplementedException();
    }
    public override ValueTask OnDisposing()
    {
        _objectRef?.Dispose();
        return base.OnDisposing();
    }
    /// <summary>
    /// Create an observer, passing the service callbacks to trigger.
    /// </summary>
    /// <param name="observerId">The observer instance id</param>
    /// <param name="onIntersectUpdate">On intersection update, pass the entries</param>
    /// <returns>The observer instance</returns>
    IntersectionObserver CreateObserver(string observerId, Action<IList<IntersectionObserverEntry>> onIntersectUpdate)
    {
        var observer = new IntersectionObserver(
            observerId,
            onIntersectUpdate,
            ObserveElement,
            Unobserve,
            Disconnect,
            RemoveObserver
        );

        if (_observers.TryAdd(observerId, observer))
        {
            return observer;
        }

        throw new Exception($"Failed to create observer for key: {observerId}");
    }
    IntersectionObserver CreateObserver(string observerId, Func<IList<IntersectionObserverEntry>, Task> onIntersectUpdate)
    {
        var observer = new IntersectionObserver(
            observerId,
            onIntersectUpdate,
            ObserveElement,
            Unobserve,
            Disconnect,
            RemoveObserver
        );

        if (_observers.TryAdd(observerId, observer))
        {
            return observer;
        }

        throw new Exception($"Failed to create observer for key: {observerId}");
    }
    /// <summary>
    /// Disconnect the observer instance
    /// </summary>
    /// <param name="id">The observer instance id</param>
    async ValueTask<bool> Disconnect(string id)
    {
        return await InvokeAsync<bool>("disconnect", id);
    }
    void EnsureObserverExists(string id)
    {
        if (!_observers.ContainsKey(id))
        {
            throw new Exception($"There is no observer for key: {id}");
        }
    }
    /// <summary>
    /// Observe an element using an observer instance
    /// </summary>
    /// <param name="id">The observer instance id</param>
    /// <param name="element">The element to observe</param>
    async ValueTask ObserveElement(string id, ElementReference element)
    {
        if (_observers.ContainsKey(id))
        {
            await InvokeAsync<object>("observe", id, element);
        }
    }
    /// <summary>
    /// Disconnect the observer instance
    /// </summary>
    /// <param name="id">The observer instance id</param>
    async ValueTask RemoveObserver(string id)
    {
        var disposed = await InvokeAsync<bool>("remove", id);

        if (disposed)
        {
            _observers.TryRemove(id, out _);
        }
    }
    /// <summary>
    /// Unobserve an element on an observer instance
    /// </summary>
    /// <param name="id">The observer instance id</param>
    /// <param name="element">The element to unobserve</param>
    async ValueTask Unobserve(string id, ElementReference element)
    {
        await InvokeAsync<bool>("unobserve", id, element);
    }
}
