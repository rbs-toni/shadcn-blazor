using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace ShadcnBlazor;
public partial class DialogProvider : IAsyncDisposable
{
    const string JSFile = "./_content/ShadcnBlazor/Components/DialogProvider.razor.js";
    readonly DialogOptions _globalDialogOptions = new();
    readonly Collection<IDialogReference> _modals = [];
    bool _haveActiveDialogs;
    IJSObjectReference? _styleFunctions;

    internal event Action? OnDialogClosed;

    [Inject]
    IJSRuntime JsRuntime { get; set; } = default!;
    [Inject]
    NavigationManager NavigationManager { get; set; } = default!;

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        if (_styleFunctions is not null)
        {
            try
            {
                await _styleFunctions.DisposeAsync();
            }
            catch (JSDisconnectedException)
            {
                // If the browser is gone, we don't need it to clean up any browser-side state
            }
        }
    }

    internal async Task CloseInstance(IDialogReference? modal, DialogResult result)
    {
        if (modal?.DialogInstanceRef != null)
        {
            // Gracefully close the modal
            await modal.DialogInstanceRef.CloseAsync(result);
            if (!_modals.Any())
            {
                await ClearBodyStyles();
            }
            OnDialogClosed?.Invoke();
        }
        else
        {
            await DismissInstance(modal, result);
        }
    }
    internal Task DismissInstance(Guid id, DialogResult result)
    {
        var reference = GetIDialogReference(id);
        return DismissInstance(reference, result);
    }
    internal async Task DismissInstance(IDialogReference? modal, DialogResult result)
    {
        if (modal != null)
        {
            modal.Dismiss(result);
            _modals.Remove(modal);
            if (_modals.Count == 0)
            {
                await ClearBodyStyles();
            }
            await InvokeAsync(StateHasChanged);
            OnDialogClosed?.Invoke();
        }
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _styleFunctions = await JsRuntime.InvokeAsync<IJSObjectReference>("import", JSFile);
        }
    }
    protected override void OnInitialized()
    {
        if (CascadedDialogService == null)
        {
            throw new InvalidOperationException($"{GetType()} requires a cascading parameter of type {nameof(IDialogService)}.");
        }

        ((DialogService)CascadedDialogService).OnDialogInstanceAdded += Update;
        ((DialogService)CascadedDialogService).OnDialogCloseRequested += CloseInstance;
        NavigationManager.LocationChanged += CancelDialogs;
    }
    async void CancelDialogs(object? sender, LocationChangedEventArgs e)
    {
        foreach (var modalReference in _modals.ToList())
        {
            modalReference.Dismiss(DialogResult.Cancel());
        }

        _modals.Clear();
        await ClearBodyStyles();
        await InvokeAsync(StateHasChanged);
    }
    async Task ClearBodyStyles()
    {
        _haveActiveDialogs = false;
        if (_styleFunctions is not null)
        {
            await _styleFunctions.InvokeVoidAsync("removeBodyStyle");
        }
    }
    IDialogReference? GetIDialogReference(Guid id)
        => _modals.SingleOrDefault(x => x.Id == id);
    async Task Update(IDialogReference modalReference)
    {
        _modals.Add(modalReference);

        if (!_haveActiveDialogs)
        {
            _haveActiveDialogs = true;
            if (_styleFunctions is not null)
            {
                await _styleFunctions.InvokeVoidAsync("setBodyStyle");
            }
        }

        await InvokeAsync(StateHasChanged);
    }
}
