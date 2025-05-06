using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using TailwindMerge.Extensions;

namespace ShadcnBlazor;
public static class ConfigureServices
{
    public static void AddShadcnBlazor(this IServiceCollection services)
    {
        services.AddScoped<IElementRect, ElementRectService>();
        services.AddScoped<IKeyCodeService, KeyCodeService>();
        services.AddTailwindMerge();
    }
    public static void AddTeleporter(this WebAssemblyHostBuilder builder)
    {
        builder.RootComponents.Add<TeleportOutlet>("body::after");
        builder.Services.AddSingleton<ITeleporter, Teleporter>();
    }
}
