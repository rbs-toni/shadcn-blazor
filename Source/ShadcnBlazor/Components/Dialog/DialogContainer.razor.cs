using Microsoft.AspNetCore.Components;

namespace ShadcnBlazor;
public partial class DialogContainer : ShadcnComponentBase, IDialogInstanceInternal
{
    readonly string _elementId = Identifier.NewId();
    Dialog? _dialog;
    DialogContext? _context;

    string IDialogInstance.ElementId => _elementId;

    [CascadingParameter]
    DialogProvider? Parent { get; set; }

    void IDialogInstance.Cancel() => ((IDialogInstance)this).Close(DialogResult.Cancel());
    void IDialogInstance.CancelAll()
    {
        Parent?.DismissAll();
    }
    void IDialogInstance.Close()
    {
        ((IDialogInstance)this).Close(DialogResult.Ok<object?>(null));
    }
    void IDialogInstance.Close(DialogResult dialogResult)
    {
        Parent?.DismissInstance(Id, dialogResult);
    }
    void IDialogInstance.Close<T>(T returnValue)
    {
        var dialogResult = DialogResult.Ok(returnValue);
        Parent?.DismissInstance(Id, dialogResult);
    }
    void IDialogInstance.StateHasChanged() => StateHasChanged();
    void IDialogInstanceInternal.Register(Dialog dialog)
    {
        _dialog = dialog;
        _context = dialog.Context;
        ChildContent = dialog.ChildContent;
        StateHasChanged();
    }
}
