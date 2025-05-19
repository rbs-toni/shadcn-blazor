using Microsoft.AspNetCore.Components;

namespace ShadcnBlazor;
public class ToastService : IToastService
{
    public event Action? OnClearAll;
    public event Action? OnClearQueue;
    public event Action<ToastType>? OnClearQueueToasts;
    public event Action<ToastType>? OnClearToasts;
    public event Action<ToastInstance>? OnShow;

    public void ClearAll()
    {
        throw new NotImplementedException();
    }

    public void ClearErrorToasts()
    {
        throw new NotImplementedException();
    }

    public void ClearInfoToasts()
    {
        throw new NotImplementedException();
    }

    public void ClearQueue()
    {
        throw new NotImplementedException();
    }

    public void ClearQueueErrorToasts()
    {
        throw new NotImplementedException();
    }

    public void ClearQueueInfoToasts()
    {
        throw new NotImplementedException();
    }

    public void ClearQueueSuccessToasts()
    {
        throw new NotImplementedException();
    }

    public void ClearQueueToasts(ToastType toastType)
    {
        throw new NotImplementedException();
    }

    public void ClearQueueWarningToasts()
    {
        throw new NotImplementedException();
    }

    public void ClearSuccessToasts()
    {
        throw new NotImplementedException();
    }

    public void ClearToasts(ToastType toastType)
    {
        throw new NotImplementedException();
    }

    public void ClearWarningToasts()
    {
        throw new NotImplementedException();
    }

    public void Show(string title, Action<ToastParameters>? parameters = null)
    {
        var toastParameters = new ToastParameters();
        parameters?.Invoke(toastParameters);
        var instance = ToastInstance.Create(title, ToastType.Default, toastParameters);
        OnShow?.Invoke(instance);
    }

    public void ShowError(string title, Action<ToastParameters>? settings = null)
    {
        throw new NotImplementedException();
    }

    public void ShowInfo(string title, Action<ToastParameters>? settings = null)
    {
        throw new NotImplementedException();
    }

    public void ShowSuccess(string title, Action<ToastParameters>? settings = null)
    {
        throw new NotImplementedException();
    }

    public void ShowWarning(string title, Action<ToastParameters>? settings = null)
    {
        throw new NotImplementedException();
    }
}
