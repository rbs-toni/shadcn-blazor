using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShadcnBlazor.Docs;

public static class NavigationUtils
{
    /// <summary>
    /// Determines whether the current route matches the given link or any additional paths.
    /// </summary>
    /// <param name="navigationManager">Injected NavigationManager</param>
    /// <param name="link">Primary link to match (e.g., "/docs")</param>
    /// <param name="match">Whether to match prefix or exact</param>
    /// <param name="additionalMatches">Optional additional paths to match</param>
    public static bool IsActiveLink(
        NavigationManager navigationManager,
        string? link,
        NavLinkMatch match = NavLinkMatch.Prefix,
        IEnumerable<string>? additionalMatches = null)
    {
        if (string.IsNullOrWhiteSpace(link))
        {
            return false;
        }

        var relativePath = navigationManager.ToBaseRelativePath(navigationManager.Uri);
        var normalizedLink = link.TrimStart('/');
        var normalizedPath = relativePath.TrimStart('/');

        if (additionalMatches?.Any() == true)
        {
            var normalizedMatches = additionalMatches
                .Select(m => m.TrimStart('/'))
                .ToList();

            foreach (var m in normalizedMatches)
                Console.WriteLine($"    - {m}");

            if (normalizedMatches.Contains(normalizedPath, StringComparer.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        bool result = match == NavLinkMatch.All
            ? string.Equals(normalizedPath, normalizedLink, StringComparison.OrdinalIgnoreCase)
            : normalizedPath.StartsWith(normalizedLink, StringComparison.OrdinalIgnoreCase);

        return result;
    }
}

