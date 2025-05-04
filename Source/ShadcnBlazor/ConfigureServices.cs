using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace ShadcnBlazor;
public static class ConfigureServices
{
    public static void AddShadcnBlazor(this IServiceCollection services)
    {
        services.AddScoped<IElementRect, ElementRectService>();
        services.AddScoped<IKeyCodeService, KeyCodeService>();
    }
}
