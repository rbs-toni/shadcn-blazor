using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace ShadcnBlazor;
public partial class DialogInstance : IDisposable
{
    private bool _disableNextRender;
    [Parameter]
    public Guid Id { get; set; }
    protected override bool ShouldRender()
    {
        if (!_disableNextRender)
        {
            return true;
        }

        _disableNextRender = false;
        return false;
    }
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
    public void Dispose()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Closes the modal with a default Ok result />.
    /// </summary>
    public async Task CloseAsync()
        => await CloseAsync(DialogResult.Ok());

    /// <summary>
    /// Closes the modal with the specified <paramref name="modalResult"/>.
    /// </summary>
    /// <param name="modalResult"></param>
    public async Task CloseAsync(DialogResult modalResult)
    {
        await Parent.DismissInstance(Id, modalResult);
    }
    [CascadingParameter]
    DialogProvider Parent { get; set; } = default!;
    /// <summary>
    /// Closes the modal and returns a cancelled DialogResult.
    /// </summary>
    public async Task CancelAsync()
        => await CloseAsync(DialogResult.Cancel());

    /// <summary>
    /// Closes the modal returning the specified <paramref name="payload"/> in a cancelled DialogResult.
    /// </summary>
    public async Task CancelAsync<TPayload>(TPayload payload)
        => await CloseAsync(DialogResult.Cancel(payload));
}