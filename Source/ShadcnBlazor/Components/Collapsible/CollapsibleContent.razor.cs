using Microsoft.AspNetCore.Components;
using System;
using System.Linq;

namespace ShadcnBlazor;
public partial class CollapsibleContent
{
    bool Open => Collapsible?.Open ?? false;
    bool Disabled => Collapsible?.Disabled ?? false;

    [Inject]
    IElementRect ElementRectService { get; set; } = default!;

    protected double? Width { get; set; }
    protected double? Height { get; set; }

    bool _hasMeasured;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (Open && !_hasMeasured && !string.IsNullOrWhiteSpace(Ref.Id))
        {
            try
            {
                var rect = await ElementRectService.GetBoundingClientRectAsync(Ref);
                if (rect != null)
                {
                    Width = rect.Width;
                    Height = rect.Height;
                    _hasMeasured = true;
                    StateHasChanged(); // force re-render with measured dimensions
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to get bounding rect: {ex.Message}");
            }
        }
    }
}
