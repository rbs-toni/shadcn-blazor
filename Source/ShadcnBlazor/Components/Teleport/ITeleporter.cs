using Microsoft.AspNetCore.Components;
using System;
using System.Linq;

namespace ShadcnBlazor;
public interface ITeleporter
{
    void Add(RenderFragment content);
    void Remove(RenderFragment content);
    void RemoveAll();
    event Action? OnTeleported;
    event Action? OnRemoved;
    IReadOnlyList<RenderFragment> Contents { get; }
}

public class Teleporter : ITeleporter
{
    private readonly List<RenderFragment> _contents = [];

    public IReadOnlyList<RenderFragment> Contents => _contents.AsReadOnly();

    public event Action? OnTeleported;
    public event Action? OnRemoved;

    public void Add(RenderFragment fragment)
    {
        // Add the fragment only if it is not already in the list
        if (!_contents.Contains(fragment))
        {
            _contents.Add(fragment);
            OnTeleported?.Invoke();
        }
    }

    public void Remove(RenderFragment fragment)
    {
        // Remove the fragment if it exists in the list
        if (_contents.Contains(fragment))
        {
            _contents.Remove(fragment);
            OnRemoved?.Invoke();
        }
    }

    public void RemoveAll()
    {
        _contents.Clear();
    }
}
