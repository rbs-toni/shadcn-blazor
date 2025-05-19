using Microsoft.AspNetCore.Components;

namespace ShadcnBlazor;
public interface IToastService
{
    /// <summary>
    /// A event that will be invoked to clear all toasts
    /// </summary>
    event Action? OnClearAll;
    /// <summary>
    /// A event that will be invoked to clear all queued toasts
    /// </summary>
    event Action? OnClearQueue;
    /// <summary>
    /// A event that will be invoked to clear queue toast of specified level
    /// </summary>
    event Action<ToastType>? OnClearQueueToasts;
    /// <summary>
    /// A event that will be invoked to clear toast of specified level
    /// </summary>
    event Action<ToastType>? OnClearToasts;
    /// <summary>
    /// A event that will be invoked when showing a toast
    /// </summary>
    event Action<ToastInstance>? OnShow;

    /// <summary>
    /// Removes all toasts
    /// </summary>
    void ClearAll();
    /// <summary>
    /// Removes all toasts with toast level Error
    /// </summary>
    void ClearErrorToasts();
    /// <summary>
    /// Removes all toasts with toast level Info
    /// </summary>
    void ClearInfoToasts();
    /// <summary>
    /// Removes all queued toasts
    /// </summary>
    void ClearQueue();
    /// <summary>
    /// Removes all queued toasts with toast level Error
    /// </summary>
    void ClearQueueErrorToasts();
    /// <summary>
    /// Removes all queued toasts with toast level Info
    /// </summary>
    void ClearQueueInfoToasts();
    /// <summary>
    /// Removes all queued toasts with toast level Success
    /// </summary>
    void ClearQueueSuccessToasts();
    /// <summary>
    /// Removes all queued toasts with a specified <paramref name="toastType"/>.
    /// </summary>
    void ClearQueueToasts(ToastType toastType);
    /// <summary>
    /// Removes all queued toasts with toast level warning
    /// </summary>
    void ClearQueueWarningToasts();
    /// <summary>
    /// Removes all toasts with toast level Success
    /// </summary>
    void ClearSuccessToasts();
    /// <summary>
    /// Removes all toasts with a specified <paramref name="toastType"/>.
    /// </summary>
    void ClearToasts(ToastType toastType);
    /// <summary>
    /// Removes all toasts with toast level warning
    /// </summary>
    void ClearWarningToasts();
    void Show(string title, Action<ToastParameters>? parameters = null);
    void ShowSuccess(string title, Action<ToastParameters>? settings = null);
    void ShowInfo(string title, Action<ToastParameters>? settings = null);
    void ShowWarning(string title, Action<ToastParameters>? settings = null);
    void ShowError(string title, Action<ToastParameters>? settings = null);
}
