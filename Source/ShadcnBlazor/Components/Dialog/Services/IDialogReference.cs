using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace ShadcnBlazor;
public interface IDialogReference
{
    /// <summary>
    /// The unique ID of this dialog.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// The content within this dialog.
    /// </summary>
    RenderFragment? RenderFragment { get; set; }

    /// <summary>
    /// The result of closing the dialog.
    /// </summary>
    Task<DialogResult?> Result { get; }

    TaskCompletionSource<bool> RenderCompleteTaskCompletionSource { get; }

    /// <summary>
    /// Hides the dialog.
    /// </summary>
    void Close();

    /// <summary>
    /// Hides the dialog and returns a result.
    /// </summary>
    /// <param name="result">The result of closing the dialog.</param>
    void Close(DialogResult? result);

    /// <summary>
    /// Notifies that this dialog has been dismissed.
    /// </summary>
    /// <param name="result">The result of closing the dialog.</param>
    /// <returns>Returns <c>true</c> if the result was set successfully.</returns>
    bool Dismiss(DialogResult? result);

    /// <summary>
    /// The dialog linked to this reference.
    /// </summary>
    object? Dialog { get; }

    /// <summary>
    /// Replaces the dialog content.
    /// </summary>
    /// <param name="rf">The new content to use.</param>
    void InjectRenderFragment(RenderFragment rf);

    /// <summary>
    /// Replaces the dialog with the specified reference.
    /// </summary>
    /// <param name="inst">The new dialog to use.</param>
    void InjectDialog(object inst);

    /// <summary>
    /// Gets the result of closing the dialog.
    /// </summary>
    /// <typeparam name="T">The type of value to return.</typeparam>
    /// <returns>The results of closing the dialog.</returns>
    Task<T?> GetReturnValueAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>();
}
