using System.Reflection;

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
