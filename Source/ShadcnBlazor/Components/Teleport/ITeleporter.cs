using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShadcnBlazor;

public interface ITeleporter
{
    void Teleport(RenderFragment fragment);
    void Remove(RenderFragment fragment);
    event Action? OnTeleported;
    IReadOnlyList<RenderFragment> Fragments { get; }
}
