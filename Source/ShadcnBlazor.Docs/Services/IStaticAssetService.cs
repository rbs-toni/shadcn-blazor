namespace ShadcnBlazor.Docs;
public interface IStaticAssetService
{
    Task<string?> GetAsync(string assetUrl, bool useCache = true);
}
