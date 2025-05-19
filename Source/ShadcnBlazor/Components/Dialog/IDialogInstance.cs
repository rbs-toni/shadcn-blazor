namespace ShadcnBlazor;
public interface IDialogInstance
{

    /// <summary>
    /// The unique ID for this instance.
    /// </summary>
    string? Id { get; }

    /// <summary>
    /// The unique HTML element ID of the dialog container (mud-dialog-container).
    /// </summary>
    string ElementId { get; }

    /// <summary>
    /// Closes this dialog with a result of <c>DialogResult.Ok</c>.
    /// </summary>
    void Close();

    /// <summary>
    /// Closes this dialog with a custom result.
    /// </summary>
    /// <param name="dialogResult">The result to include, such as <see cref="DialogResult.Ok{T}(T)"/> or <see cref="DialogResult.Cancel"/>.</param>
    void Close(DialogResult dialogResult);

    /// <summary>
    /// Closes this dialog with a custom return value.
    /// </summary>
    /// <typeparam name="T">The type of value being returned.</typeparam>
    /// <param name="returnValue">The custom value to include.</param>
    void Close<T>(T returnValue);

    /// <summary>
    /// Closes this dialog with a result of <c>DialogResult.Cancel</c>.
    /// </summary>
    void Cancel();

    /// <summary>
    /// Closes this dialog and any parent dialogs.
    /// </summary>
    void CancelAll();

    /// <summary>
    /// Notifies the component that its state has changed. When applicable, this will
    /// cause the component to be re-rendered.
    /// </summary>
    void StateHasChanged();
}
