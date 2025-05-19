using Microsoft.AspNetCore.Components;

namespace ShadcnBlazor;
public static class ElementReferenceExtensions
{
    #region RTL
    public static async Task<bool> IsRtlAsync(this ElementReference element, IElementService service)
    {
        return await service.IsRtlAsync(element);
    }
    #endregion

    #region DOM Measurement
    public static async Task<DomRect> GetBoundingClientRectAsync(this ElementReference element, IElementService service)
    {
        return await service.GetBoundingClientRectAsync(element);
    }

    public static async Task<double> GetOffsetTopAsync(this ElementReference element, IElementService service)
    {
        return await service.GetOffsetTopAsync(element);
    }

    public static async Task<double> GetOffsetLeftAsync(this ElementReference element, IElementService service)
    {
        return await service.GetOffsetLeftAsync(element);
    }

    public static async Task<double> GetScrollTopAsync(this ElementReference element, IElementService service)
    {
        return await service.GetScrollTopAsync(element);
    }

    public static async Task SetScrollTopAsync(this ElementReference element, IElementService service, double value)
    {
        await service.SetScrollTopAsync(element, value);
    }
    #endregion

    #region Visibility
    public static async Task<bool> IsElementVisibleAsync(this ElementReference element, IElementService service)
    {
        return await service.IsElementVisibleAsync(element);
    }

    public static async Task<bool> IsElementInViewportAsync(this ElementReference element, IElementService service)
    {
        return await service.IsElementInViewportAsync(element);
    }

    public static async Task ScrollIntoViewAsync(this ElementReference element, IElementService service, bool smooth = false)
    {
        await service.ScrollIntoViewAsync(element, smooth);
    }
    #endregion

    #region Focus Management
    public static async Task FocusAsync(this ElementReference element, IElementService service)
    {
        await service.FocusAsync(element);
    }

    public static async Task BlurAsync(this ElementReference element, IElementService service)
    {
        await service.BlurAsync(element);
    }

    public static async Task<bool> HasFocusAsync(this ElementReference element, IElementService service)
    {
        return await service.HasFocusAsync(element);
    }
    #endregion

    #region CSS Classes
    public static async Task AddClassAsync(this ElementReference element, IElementService service, string className)
    {
        await service.AddClassAsync(element, className);
    }

    public static async Task RemoveClassAsync(this ElementReference element, IElementService service, string className)
    {
        await service.RemoveClassAsync(element, className);
    }

    public static async Task ToggleClassAsync(this ElementReference element, IElementService service, string className)
    {
        await service.ToggleClassAsync(element, className);
    }

    public static async Task<bool> ContainsClassAsync(this ElementReference element, IElementService service, string className)
    {
        return await service.ContainsClassAsync(element, className);
    }
    #endregion

    #region Attributes
    public static async Task SetAttributeAsync(this ElementReference element, IElementService service, string name, string value)
    {
        await service.SetAttributeAsync(element, name, value);
    }

    public static async Task<string> GetAttributeAsync(this ElementReference element, IElementService service, string name)
    {
        return await service.GetAttributeAsync(element, name);
    }

    public static async Task RemoveAttributeAsync(this ElementReference element, IElementService service, string name)
    {
        await service.RemoveAttributeAsync(element, name);
    }
    #endregion

    #region Styles
    public static async Task SetStyleAsync(this ElementReference element, IElementService service, string property, string value)
    {
        await service.SetStyleAsync(element, property, value);
    }

    public static async Task<string> GetStyleAsync(this ElementReference element, IElementService service, string property)
    {
        return await service.GetStyleAsync(element, property);
    }
    #endregion

    #region Events
    public static async Task AddEventListenerAsync<T>(this ElementReference element, IElementService service, string eventName, EventCallback<T> callback, object options = null) where T : EventArgs
    {
        await service.AddEventListenerAsync(element, eventName, async (e) =>
        {
            // Simple conversion - for more complex scenarios you might need proper event args conversion
            await callback.InvokeAsync((T)e);
        }, options);
    }

    public static async Task RemoveEventListenerAsync<T>(this ElementReference element, IElementService service, string eventName) where T : EventArgs
    {
        await service.RemoveEventListenerAsync(element, eventName);
    }
    #endregion

    #region Text/HTML
    public static async Task<string> GetTextContentAsync(this ElementReference element, IElementService service)
    {
        return await service.GetTextContentAsync(element);
    }

    public static async Task SetTextContentAsync(this ElementReference element, IElementService service, string content)
    {
        await service.SetTextContentAsync(element, content);
    }

    public static async Task<string> GetInnerHtmlAsync(this ElementReference element, IElementService service)
    {
        return await service.GetInnerHtmlAsync(element);
    }

    public static async Task SetInnerHtmlAsync(this ElementReference element, IElementService service, string html)
    {
        await service.SetInnerHtmlAsync(element, html);
    }
    #endregion

    #region Misc
    public static async Task ClickAsync(this ElementReference element, IElementService service)
    {
        await service.ClickAsync(element);
    }

    public static async Task PreventDefaultAsync(this ElementReference element, IElementService service, string eventName)
    {
        await service.PreventDefaultAsync(element, eventName);
    }

    public static async Task StopPropagationAsync(this ElementReference element, IElementService service, string eventName)
    {
        await service.StopPropagationAsync(element, eventName);
    }
    #endregion
}
