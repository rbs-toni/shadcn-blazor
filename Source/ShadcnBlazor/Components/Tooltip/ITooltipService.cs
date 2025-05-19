using Microsoft.AspNetCore.Components;
using System;

namespace ShadcnBlazor;
public interface ITooltipService
{
    event Action? OnUpdated;

    IEnumerable<RenderFragment> Tooltips { get; }

    void AddTooltip(string id, RenderFragment tooltip);
    void Clear();
    void Refresh();
    void RemoveTooltip(string id);
}

class TooltipService : ITooltipService, IDisposable
{
    readonly ReaderWriterLockSlim _lock = new();
    readonly Dictionary<string, RenderFragment> _tooltips = new();

    public event Action? OnUpdated;

    public IEnumerable<RenderFragment> Tooltips
    {
        get
        {
            _lock.EnterReadLock();
            try
            {
                return _tooltips.Values.ToList(); // Prevents collection mutation during enumeration
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }
    }

    public void AddTooltip(string id, RenderFragment tooltip)
    {
        _lock.EnterWriteLock();
        try
        {
            _tooltips[id] = tooltip;
        }
        finally
        {
            _lock.ExitWriteLock();
        }

        OnUpdated?.Invoke();
    }
    public void Clear()
    {
        _lock.EnterWriteLock();
        try
        {
            _tooltips.Clear();
        }
        finally
        {
            _lock.ExitWriteLock();
        }

        OnUpdated?.Invoke();
    }
    public void Dispose()
    {
        Clear();
        _lock.Dispose();
    }
    public void Refresh() => OnUpdated?.Invoke();
    public void RemoveTooltip(string id)
    {
        _lock.EnterWriteLock();
        try
        {
            _tooltips.Remove(id);
        }
        finally
        {
            _lock.ExitWriteLock();
        }

        OnUpdated?.Invoke();
    }
}
