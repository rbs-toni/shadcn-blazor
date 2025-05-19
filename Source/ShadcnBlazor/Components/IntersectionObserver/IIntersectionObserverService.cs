using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShadcnBlazor;
public interface IIntersectionObserverService
{
    Task<IntersectionObserver> CreateAsync(
        Action<IList<IntersectionObserverEntry>> onIntersectUpdate,
        IntersectionObserverOptions? options = null);
    Task<IntersectionObserver> CreateAsync(
        Func<IList<IntersectionObserverEntry>, Task> onIntersectUpdate,
        IntersectionObserverOptions? options = null);
    Task<IntersectionObserver> ObserveAsync(
        ElementReference element,
        Action<IList<IntersectionObserverEntry>> onIntersectUpdate,
        IntersectionObserverOptions? options = null);
    Task<IntersectionObserver> ObserveAsync(
        ElementReference element,
        Func<IList<IntersectionObserverEntry>, Task> onIntersectUpdate,
        IntersectionObserverOptions? options = null);
    void OnCallback(string id, IList<IntersectionObserverEntry> entries);
    Task OnCallbackAsync(string id, IList<IntersectionObserverEntry> entries);
}
