namespace ShadcnBlazor;
internal interface IDialogInstanceInternal : IDialogInstance
{
    /// <summary>
    /// Links a dialog with this instance.
    /// </summary>
    /// <param name="dialog">The dialog to use.</param>
    /// <remarks>
    /// This method is used internally when displaying a new dialog.
    /// </remarks>
    void Register(Dialog dialog);
}
