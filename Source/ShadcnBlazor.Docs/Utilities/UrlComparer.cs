using System;
using System.Linq;

namespace ShadcnBlazor.Docs;
static class UrlComparer
{
    public static bool ArePathsEqual(string? url1, string? url2)
    {
        if (url1 == null || url2 == null)
        {
            return false;
        }

        string path1 = GetPath(url1);
        string path2 = GetPath(url2);

        return string.Equals(path1, path2, StringComparison.OrdinalIgnoreCase);
    }

    static string GetPath(string url)
    {
        Uri uri = new(url);
        return uri.AbsolutePath;
    }

    public static List<string> GetUniquePaths(List<string> urls)
    {
        return urls
            .Select(GetPath)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }
}
