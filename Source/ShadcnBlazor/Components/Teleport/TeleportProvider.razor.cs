using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using System;
using System.Linq;

namespace ShadcnBlazor;
public partial class TeleportProvider
{
    IReadOnlyList<RenderFragment> Fragments => Teleporter.Contents;

    public void Dispose()
    {
        NavigationManager.LocationChanged -= LocationChanged;
    }
    protected override void OnInitialized()
    {
        Teleporter.OnTeleported += () => InvokeAsync(StateHasChanged);
        Teleporter.OnRemoved += () => InvokeAsync(StateHasChanged);
        NavigationManager.LocationChanged += LocationChanged;
    }
    void LocationChanged(object? sender, LocationChangedEventArgs args)
    {
        Teleporter.RemoveAll();
    }
}
