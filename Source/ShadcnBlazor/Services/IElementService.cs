using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;

namespace ShadcnBlazor;

public interface IElementService
{
    // DOM Measurement
    Task<DomRect> GetBoundingClientRectAsync(ElementReference element);
    Task<double> GetOffsetTopAsync(ElementReference element);
    Task<double> GetOffsetLeftAsync(ElementReference element);
    Task<double> GetScrollTopAsync(ElementReference element);
    Task SetScrollTopAsync(ElementReference element, double value);

    // Visibility
    Task<bool> IsElementVisibleAsync(ElementReference element);
    Task<bool> IsElementInViewportAsync(ElementReference element);
    Task ScrollIntoViewAsync(ElementReference element, bool smooth = false);

    // Focus Management
    Task FocusAsync(ElementReference element);
    Task BlurAsync(ElementReference element);
    Task<bool> HasFocusAsync(ElementReference element);

    // CSS Classes
    Task AddClassAsync(ElementReference element, string className);
    Task RemoveClassAsync(ElementReference element, string className);
    Task ToggleClassAsync(ElementReference element, string className);
    Task<bool> ContainsClassAsync(ElementReference element, string className);

    // Attributes
    Task SetAttributeAsync(ElementReference element, string name, string value);
    Task<string> GetAttributeAsync(ElementReference element, string name);
    Task RemoveAttributeAsync(ElementReference element, string name);

    // Styles
    Task SetStyleAsync(ElementReference element, string property, string value);
    Task<string> GetStyleAsync(ElementReference element, string property);

    // Events
    Task AddEventListenerAsync(
        ElementReference element,
        string eventName,
        Action<EventArgs> callback,
        object? options = null);
    Task RemoveEventListenerAsync(ElementReference element, string eventName);

    // Text/HTML
    Task<string> GetTextContentAsync(ElementReference element);
    Task SetTextContentAsync(ElementReference element, string content);
    Task<string> GetInnerHtmlAsync(ElementReference element);
    Task SetInnerHtmlAsync(ElementReference element, string html);

    // RTL
    Task<bool> IsRtlAsync(ElementReference element);

    // Window/Viewport
    Task<double> GetWindowInnerWidthAsync();
    Task<double> GetWindowInnerHeightAsync();
    Task<double> GetDevicePixelRatioAsync();

    // Clipboard
    Task CopyToClipboardAsync(string text);
    Task<string> ReadFromClipboardAsync();

    // Misc
    Task ClickAsync(ElementReference element);
    Task PreventDefaultAsync(ElementReference element, string eventName);
    Task StopPropagationAsync(ElementReference element, string eventName);
}

public class ElementService : JSModule, IElementService
{
    readonly Dictionary<string, JSEventCallback> _references = [];
    public ElementService(IJSRuntime js) : base(js, "./_content/ShadcnBlazor/modules/element.min.js")
    {
    }

    // DOM Measurement
    public async Task<DomRect> GetBoundingClientRectAsync(ElementReference element)
    { return await InvokeAsync<DomRect>("getBoundingClientRect", element); }

    public async Task<double> GetOffsetTopAsync(ElementReference element)
    { return await InvokeAsync<double>("getOffsetTop", element); }

    public async Task<double> GetOffsetLeftAsync(ElementReference element)
    { return await InvokeAsync<double>("getOffsetLeft", element); }

    public async Task<double> GetScrollTopAsync(ElementReference element)
    { return await InvokeAsync<double>("getScrollTop", element); }

    public async Task SetScrollTopAsync(ElementReference element, double value)
    { await InvokeVoidAsync("setScrollTop", element, value); }

    // Visibility
    public async Task<bool> IsElementVisibleAsync(ElementReference element)
    { return await InvokeAsync<bool>("isElementVisible", element); }

    public async Task<bool> IsElementInViewportAsync(ElementReference element)
    { return await InvokeAsync<bool>("isElementInViewport", element); }

    public async Task ScrollIntoViewAsync(ElementReference element, bool smooth = false)
    { await InvokeVoidAsync("scrollIntoView", element, smooth); }

    // Focus Management
    public async Task FocusAsync(ElementReference element) { await InvokeVoidAsync("focus", element); }

    public async Task BlurAsync(ElementReference element) { await InvokeVoidAsync("blur", element); }

    public async Task<bool> HasFocusAsync(ElementReference element)
    { return await InvokeAsync<bool>("hasFocus", element); }

    // CSS Classes
    public async Task AddClassAsync(ElementReference element, string className)
    { await InvokeVoidAsync("addClass", element, className); }

    public async Task RemoveClassAsync(ElementReference element, string className)
    { await InvokeVoidAsync("removeClass", element, className); }

    public async Task ToggleClassAsync(ElementReference element, string className)
    { await InvokeVoidAsync("toggleClass", element, className); }

    public async Task<bool> ContainsClassAsync(ElementReference element, string className)
    { return await InvokeAsync<bool>("containsClass", element, className); }

    // Attributes
    public async Task SetAttributeAsync(ElementReference element, string name, string value)
    { await InvokeVoidAsync("setAttribute", element, name, value); }

    public async Task<string> GetAttributeAsync(ElementReference element, string name)
    { return await InvokeAsync<string>("getAttribute", element, name); }

    public async Task RemoveAttributeAsync(ElementReference element, string name)
    { await InvokeVoidAsync("removeAttribute", element, name); }

    // Styles
    public async Task SetStyleAsync(ElementReference element, string property, string value)
    { await InvokeVoidAsync("setStyle", element, property, value); }

    public async Task<string> GetStyleAsync(ElementReference element, string property)
    { return await InvokeAsync<string>("getStyle", element, property); }

    // Events
    public async Task AddEventListenerAsync(
        ElementReference element,
        string eventName,
        Action<EventArgs> callback,
        object? options = null)
    {
        var key = element.Id + eventName;
        if (_references.TryGetValue(key, out var existing))
        {
            if (existing.Callback == callback)
            {
                // Nothing to do because its already registered
                return;
            }
            // Remove the event listener
            await RemoveEventListenerAsync(element, eventName);
        }
        existing?.ChangeCallback(callback);

        await InvokeVoidAsync("addEventListener", element, eventName, existing?.DotNetObject, options);
    }

    public async Task RemoveEventListenerAsync(ElementReference element, string eventName)
    {
        var key = element.Id + eventName;
        var reference = _references[key];

        await InvokeVoidAsync("removeEventListener", element, eventName, reference.DotNetObject);
    }

    // Text/HTML
    public async Task<string> GetTextContentAsync(ElementReference element)
    { return await InvokeAsync<string>("getTextContent", element); }

    public async Task SetTextContentAsync(ElementReference element, string content)
    { await InvokeVoidAsync("setTextContent", element, content); }

    public async Task<string> GetInnerHtmlAsync(ElementReference element)
    { return await InvokeAsync<string>("getInnerHtml", element); }

    public async Task SetInnerHtmlAsync(ElementReference element, string html)
    { await InvokeVoidAsync("setInnerHtml", element, html); }

    // RTL
    public async Task<bool> IsRtlAsync(ElementReference element) { return await InvokeAsync<bool>("isRtl", element); }

    // Window/Viewport
    public async Task<double> GetWindowInnerWidthAsync() { return await InvokeAsync<double>("getWindowInnerWidth"); }

    public async Task<double> GetWindowInnerHeightAsync() { return await InvokeAsync<double>("getWindowInnerHeight"); }

    public async Task<double> GetDevicePixelRatioAsync() { return await InvokeAsync<double>("getDevicePixelRatio"); }

    // Clipboard
    public async Task CopyToClipboardAsync(string text) { await InvokeVoidAsync("copyToClipboard", text); }

    public async Task<string> ReadFromClipboardAsync() { return await InvokeAsync<string>("readFromClipboard"); }

    // Misc
    public async Task ClickAsync(ElementReference element) { await InvokeVoidAsync("click", element); }

    public async Task PreventDefaultAsync(ElementReference element, string eventName)
    { await InvokeVoidAsync("preventDefault", element, eventName); }

    public async Task StopPropagationAsync(ElementReference element, string eventName)
    { await InvokeVoidAsync("stopPropagation", element, eventName); }
}
