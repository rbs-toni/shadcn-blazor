using Microsoft.AspNetCore.Components;

namespace ShadcnBlazor;
public interface IDialogService
{
    /// <summary>
    /// Shows a modal containing the specified <typeparamref name="TComponent"/>.
    /// </summary>
    IDialogReference Show<TComponent>() where TComponent : IComponent;
    
    /// <summary>
    /// Shows a modal containing a <typeparamref name="TComponent"/> with the specified <paramref name="options"/>.
    /// </summary>
    /// <param name="options">Options to configure the modal.</param>
    IDialogReference Show<TComponent>(DialogOptions options) where TComponent : IComponent;

    /// <summary>
    /// Shows a modal containing a <typeparamref name="TComponent"/> with the specified <paramref name="parameters"/>.
    /// </summary>
    /// <param name="parameters">Key/Value collection of parameters to pass to component being displayed.</param>
    IDialogReference Show<TComponent>(DialogParameters parameters) where TComponent : IComponent;

    /// <summary>
    /// Shows a modal containing a <typeparamref name="TComponent"/> with the specified <paramref name="parameters"/>
    /// and <paramref name="options"/>.
    /// </summary>
    /// <param name="parameters">Key/Value collection of parameters to pass to component being displayed.</param>
    /// <param name="options">Options to configure the modal.</param>
    IDialogReference Show<TComponent>(DialogParameters parameters, DialogOptions options) where TComponent : IComponent;

    /// <summary>
    /// Shows a modal containing a <typeparamref name="TComponent"/> with the specified <paramref name="title"/> .
    /// </summary>
    /// <param name="title">Dialog title</param>
    IDialogReference Show<TComponent>(string title) where TComponent : IComponent;

    /// <summary>
    /// Shows a modal containing a <typeparamref name="TComponent"/> with the specified <paramref name="title"/> and <paramref name="options"/>.
    /// </summary>
    /// <param name="title">Dialog title</param>
    /// <param name="options">Options to configure the modal</param>
    IDialogReference Show<TComponent>(string title, DialogOptions options) where TComponent : IComponent;

    /// <summary>
    /// Shows a modal containing a <typeparamref name="TComponent"/> with the specified <paramref name="title"/> and <paramref name="parameters"/>.
    /// </summary>
    /// <param name="title">Dialog title</param>
    /// <param name="parameters">Key/Value collection of parameters to pass to component being displayed</param>
    IDialogReference Show<TComponent>(string title, DialogParameters parameters) where TComponent : IComponent;

    /// <summary>
    /// Shows a modal containing a <typeparamref name="TComponent"/> with the specified <paramref name="title"/>,
    /// <paramref name="parameters"/> and <paramref name="options"/>.
    /// </summary>
    /// <param name="title">Dialog title.</param>
    /// <param name="parameters">Key/Value collection of parameters to pass to component being displayed.</param>
    /// <param name="options">Options to configure the modal.</param>
    IDialogReference Show<TComponent>(string title, DialogParameters parameters, DialogOptions options) where TComponent : IComponent;

    /// <summary>
    /// Shows a modal containing a <paramref name="component"/>.
    /// </summary>
    /// <param name="component">Type of component to display.</param>
    IDialogReference Show(Type component);

    /// <summary>
    /// Shows a modal containing a <paramref name="component"/> with the specified <paramref name="title"/>.
    /// </summary>
    /// <param name="component">Type of component to display.</param>
    /// <param name="title">Dialog title.</param>
    IDialogReference Show(Type component, string title);

    /// <summary>
    /// Shows a modal containing a <paramref name="component"/> with the specified <paramref name="title"/> and <paramref name="options"/>.
    /// </summary>
    /// <param name="component">Type of component to display.</param>
    /// <param name="title">Dialog title.</param>
    /// <param name="options">Options to configure the modal.</param>
    IDialogReference Show(Type component, string title, DialogOptions options);

    /// <summary>
    /// Shows a modal containing a <paramref name="component"/> with the specified <paramref name="title"/> and <paramref name="parameters"/>.
    /// </summary>
    /// <param name="component">Type of component to display.</param>
    /// <param name="title">Dialog title.</param>
    /// <param name="parameters">Key/Value collection of parameters to pass to component being displayed.</param>
    IDialogReference Show(Type component, string title, DialogParameters parameters);

    /// <summary>
    /// Shows a modal containing a <paramref name="component"/> with the specified <paramref name="title"/>, <paramref name="parameters"/>
    /// and <paramref name="options"/>.
    /// </summary>
    /// <param name="component">Type of component to display.</param>
    /// <param name="title">Dialog title.</param>
    /// <param name="parameters">Key/Value collection of parameters to pass to component being displayed.</param>
    /// <param name="options">Options to configure the modal.</param>
    IDialogReference Show(Type component, string title, DialogParameters parameters, DialogOptions options);

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
}