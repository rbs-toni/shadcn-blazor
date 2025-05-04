using Microsoft.AspNetCore.Components;

namespace ShadcnBlazor;
public interface IDialogReference
{
     Guid Id { get; }
    Task<DialogResult> Result { get; }
    RenderFragment DialogInstance { get; }
    DialogInstance? DialogInstanceRef { get; set; }
    void Dismiss(DialogResult result);
    void Close();
    void Close(DialogResult result);
}