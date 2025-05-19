namespace ShadcnBlazor;
public interface IKeyCodeListener
{
    /// <summary>
    /// Method called when a key is pressed.
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    Task OnKeyDownAsync(KeyCodeEventArgs args);

    /// <summary>
    /// Method called when a key is unpressed.
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    Task OnKeyUpAsync(KeyCodeEventArgs args);
}
