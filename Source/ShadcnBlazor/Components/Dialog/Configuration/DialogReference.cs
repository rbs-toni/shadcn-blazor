using Microsoft.AspNetCore.Components;

namespace ShadcnBlazor;
public class DialogReference : IDialogReference
{
    readonly Action<DialogResult> _closed;
    readonly IDialogService _modalService;
    readonly TaskCompletionSource<DialogResult> _resultCompletion = new(TaskCreationOptions.RunContinuationsAsynchronously);

    public DialogReference(Guid modalInstanceId, RenderFragment modalInstance, IDialogService modalService)
    {
        Id = modalInstanceId;
        DialogInstance = modalInstance;
        _closed = HandleClosed;
        _modalService = modalService;
    }

    public Task<DialogResult> Result => _resultCompletion.Task;
    public Guid Id { get; }
    public RenderFragment DialogInstance { get; }
    public DialogInstance? DialogInstanceRef { get; set; }

    public void Close()
        => _modalService.Close(this);
    public void Close(DialogResult result)
        => _modalService.Close(this, result);
    public void Dismiss(DialogResult result)
        => _closed.Invoke(result);
    void HandleClosed(DialogResult obj)
        => _ = _resultCompletion.TrySetResult(obj);
}
