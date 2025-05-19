using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace ShadcnBlazor;
public interface IDialogService
{
    /// <summary>
    /// Occurs when a new dialog instance is created.
    /// </summary>
    event Func<IDialogReference, Task> OnDialogInstanceAddedAsync;

    /// <summary>
    /// Occurs when a request is made to close a dialog.
    /// </summary>
    event Action<IDialogReference, DialogResult?>? OnDialogCloseRequested;

    /// <summary>
    /// Creates a reference to a dialog.
    /// </summary>
    /// <returns>The dialog reference.</returns>
    IDialogReference CreateReference();

    /// <summary>
    /// Hides an existing dialog.
    /// </summary>
    /// <param name="dialog">The reference of the dialog to hide.</param>
    void Close(IDialogReference dialog);

    /// <summary>
    /// Hides an existing dialog.
    /// </summary>
    /// <param name="dialog">The reference of the dialog to hide.</param>
    /// <param name="result">The result to include.</param>
    void Close(IDialogReference dialog, DialogResult? result);

    /// <summary>
    /// Displays a dialog.
    /// </summary>
    /// <typeparam name="TComponent">The dialog to display.</typeparam>
    /// <returns>A reference to the dialog.</returns>
    Task<IDialogReference> ShowAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TComponent>()
        where TComponent : IComponent;

    /// <summary>
    /// Displays a dialog with parameters.
    /// </summary>
    /// <typeparam name="TComponent">The dialog to display.</typeparam>
    /// <param name="parameters">The custom parameters to set within the dialog.</param>
    /// <returns>A reference to the dialog.</returns>
    Task<IDialogReference> ShowAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TComponent>(
        DialogParameters parameters)
        where TComponent : IComponent;

    /// <summary>
    /// Displays a dialog.
    /// </summary>
    /// <param name="component">The dialog to display.</param>
    /// <returns>A reference to the dialog.</returns>
    Task<IDialogReference> ShowAsync([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type component);

    /// <summary>
    /// Displays a dialog with parameters.
    /// </summary>
    /// <param name="component">The dialog to display.</param>
    /// <param name="parameters">The custom parameters to set within the dialog.</param>
    /// <returns>A reference to the dialog.</returns>
    Task<IDialogReference> ShowAsync(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type component,
        DialogParameters parameters);
}
