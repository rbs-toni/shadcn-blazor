using Microsoft.AspNetCore.Components;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace ShadcnBlazor;
public class DialogReference : IDialogReference
{
    readonly IDialogService _dialogService;
    readonly TaskCompletionSource<DialogResult?> _resultCompletion = new();

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    /// <param name="dialogInstanceId">The unique ID of the dialog.</param>
    /// <param name="dialogService">The service used to manage dialogs.</param>
    public DialogReference(string dialogInstanceId, IDialogService dialogService)
    {
        Id = dialogInstanceId;
        _dialogService = dialogService;
    }

    TaskCompletionSource<bool> IDialogReference.RenderCompleteTaskCompletionSource { get; } = new();

    /// <inheritdoc />
    public object? Dialog { get; private set; }
    /// <inheritdoc />
    public string? Id { get; }
    /// <inheritdoc />
    public RenderFragment? RenderFragment { get; set; }
    /// <inheritdoc />
    public Task<DialogResult?> Result => _resultCompletion.Task;

    /// <inheritdoc />
    public void Close()
    {
        _dialogService.Close(this);
    }
    /// <inheritdoc />
    public void Close(DialogResult? result)
    {
        _dialogService.Close(this, result);
    }
    /// <inheritdoc />
    public virtual bool Dismiss(DialogResult? result)
    {
        return _resultCompletion.TrySetResult(result);
    }
    /// <inheritdoc />
    public async Task<T?> GetReturnValueAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>()
    {
        var result = await Result;
        try
        {
            return (T?)result?.Data;
        }
        catch (InvalidCastException)
        {
            Debug.WriteLine($"Could not cast return value to {typeof(T)}, returning default.");
            return default;
        }
    }
    /// <inheritdoc />
    public void InjectDialog(object inst)
    {
        Dialog = inst;
    }
    /// <inheritdoc />
    public void InjectRenderFragment(RenderFragment rf)
    {
        RenderFragment = rf;
    }
}
