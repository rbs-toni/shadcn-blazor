using Microsoft.AspNetCore.Components;

namespace ShadcnBlazor;
public class DialogService : IDialogService
{
    internal event Func<IDialogReference, Task>? OnDialogInstanceAdded;
    internal event Func<IDialogReference, DialogResult?, Task>? OnDialogCloseRequested;

    /// <summary>
    /// Shows the modal with the component type.
    /// </summary>
    public IDialogReference Show<T>() where T : IComponent 
        => Show<T>(string.Empty, [], new DialogOptions());

    /// <summary>
    /// Shows a modal containing a <typeparamref name="TComponent"/> with the specified <paramref name="options"/>.
    /// </summary>
    /// <param name="options">Options to configure the modal.</param>
    public IDialogReference Show<TComponent>(DialogOptions options) where TComponent : IComponent
        => Show<TComponent>("", [], options);

    /// <summary>
    /// Shows a modal containing a <typeparamref name="TComponent"/> with the specified <paramref name="parameters"/>.
    /// </summary>
    /// <param name="parameters">Key/Value collection of parameters to pass to component being displayed.</param>
    public IDialogReference Show<TComponent>(DialogParameters parameters) where TComponent : IComponent
        => Show<TComponent>("", parameters, new DialogOptions());

    /// <summary>
    /// Shows a modal containing a <typeparamref name="TComponent"/> with the specified <paramref name="parameters"/>
    /// and <paramref name="options"/>.
    /// </summary>
    /// <param name="parameters">Key/Value collection of parameters to pass to component being displayed.</param>
    /// <param name="options">Options to configure the modal.</param>
    public IDialogReference Show<TComponent>(DialogParameters parameters, DialogOptions options) where TComponent : IComponent
        => Show<TComponent>("", parameters, options);

    /// <summary>
    /// Shows the modal with the component type using the specified title.
    /// </summary>
    /// <param name="title">Dialog title.</param>
    public IDialogReference Show<T>(string title) where T : IComponent 
        => Show<T>(title, [], new DialogOptions());

    /// <summary>
    /// Shows the modal with the component type using the specified title.
    /// </summary>
    /// <param name="title">Dialog title.</param>
    /// <param name="options">Options to configure the modal.</param>
    public IDialogReference Show<T>(string title, DialogOptions options) where T : IComponent 
        => Show<T>(title, [], options);

    /// <summary>
    /// Shows the modal with the component type using the specified <paramref name="title"/>,
    /// passing the specified <paramref name="parameters"/>.
    /// </summary>
    /// <param name="title">Dialog title.</param>
    /// <param name="parameters">Key/Value collection of parameters to pass to component being displayed.</param>
    public IDialogReference Show<T>(string title, DialogParameters parameters) where T : IComponent 
        => Show<T>(title, parameters, new DialogOptions());

    /// <summary>
    /// Shows the modal with the component type using the specified <paramref name="title"/>,
    /// passing the specified <paramref name="parameters"/> and setting a custom CSS style.
    /// </summary>
    /// <param name="title">Dialog title.</param>
    /// <param name="parameters">Key/Value collection of parameters to pass to component being displayed.</param>
    /// <param name="options">Options to configure the modal.</param>
    public IDialogReference Show<T>(string title, DialogParameters parameters, DialogOptions options) where T : IComponent 
        => Show(typeof(T), title, parameters, options);

    /// <summary>
    /// Shows the modal with the specific component type.
    /// </summary>
    /// <param name="contentComponent">Type of component to display.</param>
    public IDialogReference Show(Type contentComponent) 
        => Show(contentComponent, string.Empty, [], new DialogOptions());

    /// <summary>
    /// Shows the modal with the component type using the specified title.
    /// </summary>
    /// <param name="contentComponent">Type of component to display.</param>
    /// <param name="title">Dialog title.</param>
    public IDialogReference Show(Type contentComponent, string title) 
        => Show(contentComponent, title, [], new DialogOptions());

    /// <summary>
    /// Shows the modal with the component type using the specified title.
    /// </summary>
    /// <param name="title">Dialog title.</param>
    /// <param name="contentComponent">Type of component to display.</param>
    /// <param name="options">Options to configure the modal.</param>
    public IDialogReference Show(Type contentComponent, string title, DialogOptions options) 
        => Show(contentComponent, title, [], options);

    /// <summary>
    /// Shows the modal with the component type using the specified <paramref name="title"/>,
    /// passing the specified <paramref name="parameters"/>.
    /// </summary>
    /// <param name="title">Dialog title.</param>
    /// <param name="contentComponent">Type of component to display.</param>
    /// <param name="parameters">Key/Value collection of parameters to pass to component being displayed.</param>
    public IDialogReference Show(Type contentComponent, string title, DialogParameters parameters) 
        => Show(contentComponent, title, parameters, new DialogOptions());

    /// <summary>
    /// Shows the modal with the component type using the specified <paramref name="title"/>,
    /// passing the specified <paramref name="parameters"/> and setting a custom CSS style.
    /// </summary>
    /// <param name="contentComponent">Type of component to display.</param>
    /// <param name="title">Dialog title.</param>
    /// <param name="parameters">Key/Value collection of parameters to pass to component being displayed.</param>
    /// <param name="options">Options to configure the modal.</param>
    public IDialogReference Show(Type contentComponent, string title, DialogParameters parameters, DialogOptions options)
    {
        if (!typeof(IComponent).IsAssignableFrom(contentComponent))
        {
            throw new ArgumentException($"{contentComponent.FullName} must be a Blazor Component");
        }

        IDialogReference? modalReference = null;
        var modalInstanceId = Guid.NewGuid();
        var modalContent = new RenderFragment(builder =>
        {
            var i = 0;
            builder.OpenComponent(i++, contentComponent);
            foreach (var (name, value) in parameters.Parameters)
            {
                builder.AddAttribute(i++, name, value);
            }
            builder.CloseComponent();
        });
        var modalInstance = new RenderFragment(builder =>
        {
            builder.OpenComponent<DialogInstance>(0);
            builder.SetKey("modalInstanceId");
            builder.AddAttribute(1, "Options", options);
            builder.AddAttribute(2, "Title", title);
            builder.AddAttribute(3, "Content", modalContent);
            builder.AddAttribute(4, "Id", modalInstanceId);
            builder.AddComponentReferenceCapture(5, compRef => modalReference!.DialogInstanceRef = (DialogInstance)compRef);
            builder.CloseComponent();
        });
        modalReference = new DialogReference(modalInstanceId, modalInstance, this);

        OnDialogInstanceAdded?.Invoke(modalReference);

        return modalReference;
    }

    public void Close(IDialogReference modal) 
        => Close(modal, DialogResult.Ok());

    public void Close(IDialogReference modal, DialogResult? result) 
        => OnDialogCloseRequested?.Invoke(modal, result);
}