using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics.CodeAnalysis;

namespace ShadcnBlazor;
public class DialogService : IDialogService
{
    public event Action<IDialogReference, DialogResult?>? OnDialogCloseRequested;
    public event Func<IDialogReference, Task>? OnDialogInstanceAddedAsync;

    public void Close(IDialogReference dialog)
    {
        Close(dialog, DialogResult.Ok<object?>(null));
    }
    public virtual void Close(IDialogReference dialog, DialogResult? result)
    {
        OnDialogCloseRequested?.Invoke(dialog, result);
    }
    public virtual IDialogReference CreateReference()
    {
        return new DialogReference(Identifier.NewId(), this);
    }
    public Task<IDialogReference> ShowAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TComponent>()
        where TComponent : IComponent
    {
        return ShowAsync<TComponent>(DialogParameters.Default);
    }
    public Task<IDialogReference> ShowAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TComponent>(
        DialogParameters parameters) where TComponent : IComponent
    {
        return ShowAsync(typeof(TComponent), parameters);
    }
    public Task<IDialogReference> ShowAsync([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type contentComponent)
    {
        return ShowAsync(contentComponent, DialogParameters.Default);
    }
    public async Task<IDialogReference> ShowAsync(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type contentComponent,
        DialogParameters parameters)
    {
        var dialogReference = await ShowCoreAsync(contentComponent, parameters);

        // Do not wait forever, what if render fails because of some internal exception
        var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        var token = cancellationTokenSource.Token;

        await using (token.Register(() => dialogReference.RenderCompleteTaskCompletionSource.TrySetResult(false)))
        {
            await dialogReference.RenderCompleteTaskCompletionSource.Task;
            return dialogReference;
        }
    }
    async Task<IDialogReference> ShowCoreAsync(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type contentComponent,
        DialogParameters parameters)
    {
        if (!typeof(IComponent).IsAssignableFrom(contentComponent))
        {
            throw new ArgumentException($"{contentComponent.FullName} must be a Blazor IComponent");
        }


        var dialogReference = CreateReference();

        var dialogContent = DialogHelperComponent.Wrap(builder =>
        {
            var i = 0;
            builder.OpenComponent(i++, contentComponent);
            foreach (var parameter in parameters)
            {
                builder.AddAttribute(i++, parameter.Key, parameter.Value);
            }
            builder.AddComponentReferenceCapture(i, inst => dialogReference.InjectDialog(inst));
            builder.CloseComponent();
        });

        var dialogInstance = new RenderFragment(builder =>
        {
            builder.OpenComponent<DialogContainer>(0);
            builder.SetKey(dialogReference.Id);
            builder.AddComponentParameter(3, nameof(DialogContainer.ChildContent), dialogContent);
            builder.AddComponentParameter(4, nameof(DialogContainer.Id), dialogReference.Id);
            builder.CloseComponent();
        });

        dialogReference.InjectRenderFragment(dialogInstance);

        if (OnDialogInstanceAddedAsync is not null)
        {
            await OnDialogInstanceAddedAsync(dialogReference);
        }

        return dialogReference;
    }

    /// <summary>
    /// This internal wrapper component prevents overwriting parameters of once
    /// instantiated dialog instances
    /// See: https://github.com/MudBlazor/MudBlazor/issues/10659#issuecomment-2602911059
    /// </summary>
    class DialogHelperComponent : IComponent
    {
        const string ChildContent = nameof(ChildContent);
        RenderFragment? _renderFragment;
        RenderHandle _renderHandle;

        public static RenderFragment Wrap(RenderFragment renderFragment)
            => builder =>
            {
                builder.OpenComponent<DialogHelperComponent>(1);
                builder.AddAttribute(2, ChildContent, renderFragment);
                builder.CloseComponent();
            };

        void IComponent.Attach(RenderHandle renderHandle) => _renderHandle = renderHandle;
        Task IComponent.SetParametersAsync(ParameterView parameters)
        {
            if (_renderFragment is null && parameters.TryGetValue<RenderFragment>(ChildContent, out var renderFragment))
            {
                _renderFragment = renderFragment;
                _renderHandle.Render(_renderFragment);
            }
            return Task.CompletedTask;
        }
    }
}
