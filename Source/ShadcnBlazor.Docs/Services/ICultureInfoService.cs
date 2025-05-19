namespace ShadcnBlazor.Docs;
public interface ICultureInfoService
{
    Task<string> GetLanguageAsync();
    Task SetLanguageAsync(string language);
}
