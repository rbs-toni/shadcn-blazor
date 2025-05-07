using Microsoft.AspNetCore.Components;

namespace ShadcnBlazor;

public class Teleporter : ITeleporter
{
    private readonly List<RenderFragment> _fragments = new();

    public IReadOnlyList<RenderFragment> Fragments => _fragments.AsReadOnly();

    public event Action? OnTeleported;

    public void Teleport(RenderFragment fragment)
    {
        // Add the fragment only if it is not already in the list
        if (!_fragments.Contains(fragment))
        {
            _fragments.Add(fragment);
            OnTeleported?.Invoke();
        }
    }

    public void Remove(RenderFragment fragment)
    {
        // Remove the fragment if it exists in the list
        if (_fragments.Contains(fragment))
        {
            _fragments.Remove(fragment);
            OnTeleported?.Invoke();
        }
    }
}
