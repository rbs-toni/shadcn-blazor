using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using TailwindMerge.Extensions;

namespace ShadcnBlazor;
public static class ConfigureServices
{
    public static void AddShadcnBlazor(this IServiceCollection services, LibraryConfiguration? configuration = null)
    {
        var serviceLifetime = configuration?.ServiceLifetime ?? ServiceLifetime.Scoped;
        if (serviceLifetime == ServiceLifetime.Transient)
        {
            throw new NotSupportedException("Transient lifetime is not supported.");
        }

        if (serviceLifetime == ServiceLifetime.Singleton)
        {
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<IKeyCodeService, KeyCodeService>();
            services.AddSingleton<IElementService, ElementService>();
            services.AddSingleton<IKeyCodeService, KeyCodeService>();
            services.AddSingleton<ITeleporter, Teleporter>();
            services.AddSingleton<ITooltipService, TooltipService>();
            services.AddSingleton<IDocumentService, DocumentService>();
            services.AddSingleton<IToastService, ToastService>();
        }
        else
        {
            services.AddScoped<IDialogService, DialogService>();
            services.AddScoped<IKeyCodeService, KeyCodeService>();
            services.AddScoped<IElementService, ElementService>();
            services.AddScoped<IKeyCodeService, KeyCodeService>();
            services.AddScoped<ITeleporter, Teleporter>();
            services.AddScoped<ITooltipService, TooltipService>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IToastService, ToastService>();
        }
        services.AddTailwindMerge();

        var options = configuration ?? new();
        services.AddSingleton(options);
    }

    public static void AddShadcnBlazor(this IServiceCollection services, Action<LibraryConfiguration> configuration)
    {
        LibraryConfiguration options = new();
        configuration.Invoke(options);

        AddShadcnBlazor(services, options);
    }
}
