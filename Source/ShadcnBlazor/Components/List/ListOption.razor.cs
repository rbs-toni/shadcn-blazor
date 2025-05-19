using Microsoft.AspNetCore.Components;

namespace ShadcnBlazor;
public partial class ListOption<TOption> : ShadcnComponentBase, IDisposable where TOption : notnull
{
    public ListOption() { Id = Identifier.NewId(); }

    /// <summary>
    /// Gets or sets a value indicating whether the element is disabled.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }
    /// <summary>
    /// Called whenever the selection changed.
    /// </summary>
    [Parameter]
    public EventCallback<string> OnSelect { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether the element is selected.
    /// </summary>
    [Parameter]
    public bool Selected { get; set; }
    /// <summary>
    /// Called whenever the selection changed.
    /// </summary>
    [Parameter]
    public EventCallback<bool> SelectedChanged { get; set; }
    /// <summary>
    /// Gets or sets the value of this option.
    /// </summary>
    [Parameter]
    public string? Value { get; set; }
    [CascadingParameter(Name = "ListContext")]
    internal InternalListContext<TOption> InternalListContext { get; set; } = default!;

    public void Dispose() => InternalListContext.Unregister(this);
    /// <summary/>
    public async Task OnClickHandlerAsync()
    {
        if (Disabled)
        {
            return;
        }

        Selected = !Selected;

        if (SelectedChanged.HasDelegate)
        {
            await SelectedChanged.InvokeAsync(Selected);
        }

        if (OnSelect.HasDelegate)
        {
            await OnSelect.InvokeAsync(Value);
        }
        else
        {
            if (InternalListContext != null && InternalListContext.ListComponent.Items is null)
            {
                if (InternalListContext.ValueChanged.HasDelegate)
                {
                    await InternalListContext.ValueChanged.InvokeAsync(Value);
                }
                if (InternalListContext.SelectedOptionChanged.HasDelegate)
                {
                    await InternalListContext.SelectedOptionChanged.InvokeAsync();
                }
            }
        }
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender &&
            Selected &&
            InternalListContext != null &&
            InternalListContext.ValueChanged.HasDelegate &&
            InternalListContext.ListComponent.Multiple)
        {
            await InternalListContext.ValueChanged.InvokeAsync(Value);
        }
    }
    protected override Task OnInitializedAsync()
    {
        InternalListContext.Register(this);

        return base.OnInitializedAsync();
    }
}
