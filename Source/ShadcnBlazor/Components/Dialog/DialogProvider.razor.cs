using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using System;
using System.Linq;

namespace ShadcnBlazor;
public partial class DialogProvider : IDisposable
{
    readonly List<IDialogReference> _dialogs = [];

    [Inject]
    IDialogService DialogService { get; set; } = default!;

    [Inject]
    NavigationManager NavigationManager { get; set; } = default!;

    /// <summary>
    /// Hides all currently visible dialogs.
    /// </summary>
    public void DismissAll()
    {
        foreach (var dialog in _dialogs.ToArray())
        {
            DismissInstance(dialog, DialogResult.Cancel());
        }
        StateHasChanged();
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    internal void DismissInstance(string? id, DialogResult result)
    {
        var reference = GetDialogReference(id);
        if (reference != null)
        {
            DismissInstance(reference, result);
        }
    }
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            NavigationManager.LocationChanged -= LocationChanged;
            DialogService.OnDialogInstanceAddedAsync -= AddInstanceAsync;
            DialogService.OnDialogCloseRequested -= DismissInstance;
        }
    }
    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            foreach (var dialogReference in _dialogs.ToArray().Where(x => !x.Result.IsCompleted))
            {
                dialogReference.RenderCompleteTaskCompletionSource.TrySetResult(true);
            }
        }

        return base.OnAfterRenderAsync(firstRender);
    }
    protected override void OnInitialized()
    {
        DialogService.OnDialogInstanceAddedAsync += AddInstanceAsync;
        DialogService.OnDialogCloseRequested += DismissInstance;
        NavigationManager.LocationChanged += LocationChanged;
    }
    Task AddInstanceAsync(IDialogReference dialog)
    {
        _dialogs.Add(dialog);
        return InvokeAsync(StateHasChanged);
    }
    void DismissInstance(IDialogReference dialog, DialogResult? result)
    {
        if (!dialog.Dismiss(result))
        {
            return;
        }

        _dialogs.Remove(dialog);
        StateHasChanged();
    }
    IDialogReference? GetDialogReference(string? id)
    {
        return _dialogs.ToArray().FirstOrDefault(x => x.Id == id);
    }
    void LocationChanged(object? sender, LocationChangedEventArgs args)
    {
        DismissAll();
    }
}
