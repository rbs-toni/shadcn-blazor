// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace ShadcnBlazor;
public partial class ShadcnKeyCodeProvider : IDisposable
{
    /// <summary>
    /// Gets or sets a way to tells the user agent that if the event does not get explicitly handled, its default action should not be taken as it normally would be.
    /// </summary>
    [Parameter]
    public bool PreventDefault { get; set; }
    [Inject]
    IKeyCodeService KeyCodeService { get; set; } = default!;

    public void Dispose()
    {
        KeyCodeService.Clear();
    }
    void KeyDownHandler(KeyCodeEventArgs args)
    {
        foreach (var listener in KeyCodeService.Listeners)
        {
            listener.OnKeyDownAsync(args);
        }
    }
    void KeyUpHandler(KeyCodeEventArgs args)
    {
        foreach (var listener in KeyCodeService.Listeners)
        {
            listener.OnKeyUpAsync(args);
        }
    }
}
