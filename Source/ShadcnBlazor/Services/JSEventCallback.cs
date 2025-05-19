using Microsoft.JSInterop;

namespace ShadcnBlazor;
public class JSEventCallback : IDisposable
{
    public Action<EventArgs> Callback { get; protected set; }
    public DotNetObjectReference<JSEventCallback> DotNetObject { get; }
    public JSEventCallback(Action<EventArgs> callback)
    {
        Callback = callback;
        DotNetObject = DotNetObjectReference.Create(this);
    }

    public void Dispose()
    {
        DotNetObject.Dispose();
    }

    public void ChangeCallback(Action<EventArgs> newCallback)
    {
        Callback = newCallback;
    }

    [JSInvokable]
    public void Handle(EventArgs args)
    {
        Callback?.Invoke(args);
    }
}
