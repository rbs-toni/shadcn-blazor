using Microsoft.AspNetCore.Components;

namespace ShadcnBlazor;
public partial class ShadcnKeyCodeProvider : IDisposable
{
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
