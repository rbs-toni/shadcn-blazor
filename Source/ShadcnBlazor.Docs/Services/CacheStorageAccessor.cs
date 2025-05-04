using Microsoft.JSInterop;
using System;
using System.Linq;

namespace ShadcnBlazor.Docs;
public class CacheStorageAccessor : IAsyncDisposable
{
    const string JSFile = "./_content/ShadcnBlazor.Docs/js/CacheStorageAccessor.js";
    readonly IAppVersionService _appVersion;
    readonly Lazy<Task<IJSObjectReference>> _moduleTask;
    string? _currentCacheVersion;

    public CacheStorageAccessor(IJSRuntime jSRuntime, IAppVersionService appVersion)
    {
        _moduleTask = new(() => jSRuntime.InvokeAsync<IJSObjectReference>("import", JSFile).AsTask());
        _appVersion = appVersion;
    }

    public async ValueTask DisposeAsync()
    {
        if (_moduleTask.IsValueCreated && !_moduleTask.Value.IsFaulted)
        {
            try
            {
                IJSObjectReference? module = await _moduleTask.Value;
                await module.DisposeAsync().ConfigureAwait(false);
            }
            catch (InvalidOperationException)
            {
                // This can be called too early when using prerendering
            }
            catch (Exception ex) when (ex is JSDisconnectedException || ex is OperationCanceledException)
            {
                // The JSRuntime side may routinely be gone already if the reason we're disposing is that
                // the client disconnected. This is not an error.
            }
        }
    }
    public async ValueTask<string> GetAsync(HttpRequestMessage requestMessage)
    {
        if (_currentCacheVersion is null)
        {
            await InitializeCacheAsync();
        }

        return await InternalGetAsync(requestMessage);
    }
    public async ValueTask<string> PutAndGetAsync(
        HttpRequestMessage requestMessage,
        HttpResponseMessage responseMessage)
    {
        var requestMethod = requestMessage.Method.Method;
        var requestBody = await GetRequestBodyAsync(requestMessage);
        var responseBody = await responseMessage.Content.ReadAsStringAsync();

        await InvokeVoidAsync("put", requestMessage.RequestUri!, requestMethod, requestBody, responseBody);

        return responseBody;
    }
    public async ValueTask PutAsync(HttpRequestMessage requestMessage, HttpResponseMessage responseMessage)
    {
        var requestMethod = requestMessage.Method.Method;
        var requestBody = await GetRequestBodyAsync(requestMessage);
        var responseBody = await responseMessage.Content.ReadAsStringAsync();

        await InvokeVoidAsync("put", requestMessage.RequestUri!, requestMethod, requestBody, responseBody);
    }
    public async ValueTask RemoveAllAsync() { await InvokeVoidAsync("removeAll"); }
    public async ValueTask RemoveAsync(HttpRequestMessage requestMessage)
    {
        var requestMethod = requestMessage.Method.Method;
        var requestBody = await GetRequestBodyAsync(requestMessage);

        await InvokeVoidAsync("remove", requestMessage.RequestUri!, requestMethod, requestBody);
    }
    protected async ValueTask<T> InvokeAsync<T>(string identifier, params object[]? args) => await (await _moduleTask.Value).InvokeAsync<T>(
        identifier,
        args);
    protected async ValueTask InvokeVoidAsync(string identifier, params object[]? args) => await (await _moduleTask.Value).InvokeVoidAsync(
        identifier,
        args);
    static async ValueTask<string> GetRequestBodyAsync(HttpRequestMessage requestMessage)
    {
        var requestBody = string.Empty;
        if (requestMessage.Content is not null)
        {
            requestBody = await requestMessage.Content.ReadAsStringAsync();
        }

        return requestBody;
    }
    async Task InitializeCacheAsync()
    {
        // last version cached is stored in appVersion
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, "appVersion");

        // get the last version cached
        var result = await InternalGetAsync(requestMessage);
        if (!result.Equals(_appVersion.Version))
        {
            // running newer version now, clear cache, and update version in cache
            await RemoveAllAsync();
            var requestBody = await GetRequestBodyAsync(requestMessage);
            await InvokeVoidAsync(
                "put",
                requestMessage.RequestUri!,
                requestMessage.Method.Method,
                requestBody,
                _appVersion.Version);
        }
        //
        _currentCacheVersion = _appVersion.Version;
    }
    async ValueTask<string> InternalGetAsync(HttpRequestMessage requestMessage)
    {
        var requestMethod = requestMessage.Method.Method;
        var requestBody = await GetRequestBodyAsync(requestMessage);

        return await InvokeAsync<string>("get", requestMessage.RequestUri!, requestMethod, requestBody);
    }
}
