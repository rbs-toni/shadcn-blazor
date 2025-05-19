using Blazored.LocalStorage;

namespace ShadcnBlazor.Docs;
class CultureInfoService : ICultureInfoService
{
    const string LocalStorageKey = "ShadcnBlazorLanguage";
    readonly ILocalStorageService _localStorage;

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
