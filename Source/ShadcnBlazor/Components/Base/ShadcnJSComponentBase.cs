using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace ShadcnBlazor;
/// <summary>
/// Base class for components that require JS interop via dynamically imported modules.
/// </summary>
public abstract class ShadcnJSComponentBase : ShadcnComponentBase, IAsyncDisposable
{
    readonly string _jsFile;
    IJSObjectReference? _jsModule;

    protected ShadcnJSComponentBase(string jsFile)
    {
        var assemblyName = GetType().Assembly.GetName().Name;
        _jsFile = $"./_content/{assemblyName}/Components/{jsFile}.razor.js";
    }
    protected bool JSAvailable { get; private set; }
    [Inject]
    IJSRuntime JSRuntime { get; set; } = default!;

    public async ValueTask DisposeAsync()
    {
        try
        {
            await OnDisposingAsync();
        }
        catch
        {
        }

        if (_jsModule is not null)
        {
            try
            {
                await _jsModule.DisposeAsync();
            }
            catch
            {
            }
        }
    }
    /// <summary>
    /// Invokes a JS function that returns a result.
    /// </summary>
    protected async ValueTask<TValue?> InvokeAsync<TValue>(string identifier, params object?[]? args)
    {
        try
        {
            if (!JSAvailable || _jsModule is null)
            {
                return default;
            }

            return await _jsModule.InvokeAsync<TValue>(identifier, args);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex.Message, ex);
            throw;
        }
    }
    /// <summary>
    /// Invokes a JS function that returns no result.
    /// </summary>
    protected async ValueTask InvokeVoidAsync(string identifier, params object?[]? args)
    {
        if (!JSAvailable || _jsModule is null)
        {
            return;
        }

        await _jsModule.InvokeVoidAsync(identifier, args);
    }
    /// <summary>
    /// Called after the JS module has been successfully imported.
    /// Override to hook into JS-ready state.
    /// </summary>
    protected virtual ValueTask OnAfterImportAsync() => ValueTask.CompletedTask;
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        try
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", _jsFile);
            JSAvailable = _jsModule != null;
            if (JSAvailable)
            {
                await OnAfterImportAsync();
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
            JSAvailable = false;
        }
    }
    /// <summary>
    /// Called before JS module is disposed.
    /// Override to clean up resources.
    /// </summary>
    protected virtual ValueTask OnDisposingAsync() => ValueTask.CompletedTask;
}
