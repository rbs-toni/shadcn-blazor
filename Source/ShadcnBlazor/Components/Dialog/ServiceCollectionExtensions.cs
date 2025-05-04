using Microsoft.Extensions.DependencyInjection;

namespace ShadcnBlazor;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDialogServices(this IServiceCollection services)
    {
        return services.AddScoped<IDialogService, DialogService>();
    }
}