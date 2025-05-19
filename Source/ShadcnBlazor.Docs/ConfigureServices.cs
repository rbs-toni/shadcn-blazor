using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ShadcnBlazor.Docs;
public static class ConfigureServices
{
    public static void AddShadcnBlazorDocs(this IServiceCollection services)
    {
        services.AddSingleton<NavProvider>();
        services.AddScoped<IScrollHelper, ScrollHelper>();
        services.TryAddSingleton<CacheStorageAccessor>();
        services.TryAddSingleton<IAppVersionService, AppVersionService>();
        services.AddHttpClient<IStaticAssetService, HttpBasedStaticAssetService>();
        services.AddBlazoredLocalStorage();
        services.AddScoped<ICultureInfoService, CultureInfoService>();
        services.AddLocalization();
    }

    public static async Task SetDefaultCultureInfoAsync(this WebAssemblyHost host)
    {
        try
        {
            var cultureInfoService = host.Services.GetRequiredService<ICultureInfoService>();
            var language = await cultureInfoService.GetLanguageAsync();
            if (string.IsNullOrWhiteSpace(language))
            {
                language = "en-US";
                await cultureInfoService.SetLanguageAsync(language);
            }
            CultureInfo cultureInfo = new CultureInfo(language);
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: {error}", ex.Message);
            throw;
        }
    }
}
