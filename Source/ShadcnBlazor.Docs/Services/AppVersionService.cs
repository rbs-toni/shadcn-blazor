using System.Reflection;
using Blazored.LocalStorage;

namespace ShadcnBlazor.Docs;
public class AppVersionService : IAppVersionService
{
    public string Version { get => GetVersionFromAssembly(); }

    static public string GetVersionFromAssembly()
    {
        string strVersion = default!;
        var versionAttribute = Assembly.GetExecutingAssembly()
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>();
        if (versionAttribute != null)
        {
            var version = versionAttribute.InformationalVersion;
            var plusIndex = version.IndexOf('+');
            strVersion = plusIndex >= 0 && plusIndex + 9 < version.Length ? version[..(plusIndex + 9)] : version;
        }

        return strVersion;
    }
}

public interface ICultureInfoService
{
    Task<string> GetLanguageAsync();
    Task SetLanguageAsync(string language);
}

class CultureInfoService : ICultureInfoService
{
    private readonly ILocalStorageService _localStorage;
    const string LocalStorageKey = "ShadcnBlazorLanguage";
    public CultureInfoService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }
    public async Task<string> GetLanguageAsync()
    {
        return await _localStorage.GetItemAsStringAsync(LocalStorageKey).ConfigureAwait(false);
    }

    public async Task SetLanguageAsync(string language)
    {
        await _localStorage.SetItemAsStringAsync(LocalStorageKey, language);
    }
}
