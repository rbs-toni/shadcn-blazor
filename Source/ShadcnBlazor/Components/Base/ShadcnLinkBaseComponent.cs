using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace ShadcnBlazor;
public abstract class ShadcnLinkBaseComponent : ShadcnComponentBase, IDisposable
{
    const string EnableMatchAllForQueryStringAndFragmentSwitchKey = "Microsoft.AspNetCore.Components.Routing.NavLink.EnableMatchAllForQueryStringAndFragment";
    static readonly bool _enableMatchAllForQueryStringAndFragment = AppContext.TryGetSwitch(EnableMatchAllForQueryStringAndFragmentSwitchKey, out var switchValue) && switchValue;
    static readonly CaseInsensitiveCharComparer CaseInsensitiveComparer = new();
    string? _hrefAbsolute;

    [Parameter]
    public string? Href { get; set; }

    [Parameter]
    public NavLinkMatch Match { get; set; }

    [Parameter]
    public string ActiveClass { get; set; } = "active";

    protected bool IsActive { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; } = default!;

    public void Dispose()
    {
        // To avoid leaking memory, it's important to detach any event handlers in Dispose()
        NavigationManager.LocationChanged -= OnLocationChanged;
    }
    protected override void OnParametersSet()
    {
        _hrefAbsolute = Href == null ? null : NavigationManager.ToAbsoluteUri(Href).AbsoluteUri;
        IsActive = ShouldMatch(NavigationManager.Uri);
    }
    protected virtual bool ShouldMatch(string uriAbsolute)
    {
        if (_hrefAbsolute == null)
        {
            return false;
        }

        var uriAbsoluteSpan = uriAbsolute.AsSpan();
        var hrefAbsoluteSpan = _hrefAbsolute.AsSpan();
        if (EqualsHrefExactlyOrIfTrailingSlashAdded(uriAbsoluteSpan, hrefAbsoluteSpan))
        {
            return true;
        }

        if (Match == NavLinkMatch.Prefix
            && IsStrictlyPrefixWithSeparator(uriAbsolute, _hrefAbsolute))
        {
            return true;
        }

        if (_enableMatchAllForQueryStringAndFragment || Match != NavLinkMatch.All)
        {
            return false;
        }

        var uriWithoutQueryAndFragment = GetUriIgnoreQueryAndFragment(uriAbsoluteSpan);
        if (EqualsHrefExactlyOrIfTrailingSlashAdded(uriWithoutQueryAndFragment, hrefAbsoluteSpan))
        {
            return true;
        }
        hrefAbsoluteSpan = GetUriIgnoreQueryAndFragment(hrefAbsoluteSpan);
        return EqualsHrefExactlyOrIfTrailingSlashAdded(uriWithoutQueryAndFragment, hrefAbsoluteSpan);
    }
    static string? CombineWithSpace(string? str1, string str2)
        => str1 == null ? str2 : $"{str1} {str2}";
    static bool EqualsHrefExactlyOrIfTrailingSlashAdded(ReadOnlySpan<char> currentUriAbsolute, ReadOnlySpan<char> hrefAbsolute)
    {
        if (currentUriAbsolute.SequenceEqual(hrefAbsolute, CaseInsensitiveComparer))
        {
            return true;
        }

        if (currentUriAbsolute.Length == hrefAbsolute.Length - 1)
        {
            // Special case: highlight links to http://host/path/ even if you're
            // at http://host/path (with no trailing slash)
            //
            // This is because the router accepts an absolute URI value of "same
            // as base URI but without trailing slash" as equivalent to "base URI",
            // which in turn is because it's common for servers to return the same page
            // for http://host/vdir as they do for host://host/vdir/ as it's no
            // good to display a blank page in that case.
            if (hrefAbsolute[hrefAbsolute.Length - 1] == '/' &&
                currentUriAbsolute.SequenceEqual(hrefAbsolute.Slice(0, hrefAbsolute.Length - 1), CaseInsensitiveComparer))
            {
                return true;
            }
        }

        return false;
    }
    static ReadOnlySpan<char> GetUriIgnoreQueryAndFragment(ReadOnlySpan<char> uri)
    {
        if (uri.IsEmpty)
        {
            return [];
        }

        var queryStartPos = uri.IndexOf('?');
        var fragmentStartPos = uri.IndexOf('#');

        if (queryStartPos < 0 && fragmentStartPos < 0)
        {
            return uri;
        }

        int minPos;
        if (queryStartPos < 0)
        {
            minPos = fragmentStartPos;
        }
        else if (fragmentStartPos < 0)
        {
            minPos = queryStartPos;
        }
        else
        {
            minPos = Math.Min(queryStartPos, fragmentStartPos);
        }

        return uri.Slice(0, minPos);
    }
    static bool IsStrictlyPrefixWithSeparator(string value, string prefix)
    {
        var prefixLength = prefix.Length;
        if (value.Length > prefixLength)
        {
            return value.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
                && (
                    // Only match when there's a separator character either at the end of the
                    // prefix or right after it.
                    // Example: "/abc" is treated as a prefix of "/abc/def" but not "/abcdef"
                    // Example: "/abc/" is treated as a prefix of "/abc/def" but not "/abcdef"
                    prefixLength == 0
                    || !IsUnreservedCharacter(prefix[prefixLength - 1])
                    || !IsUnreservedCharacter(value[prefixLength])
                );
        }
        else
        {
            return false;
        }
    }
    static bool IsUnreservedCharacter(char c)
    {
        // Checks whether it is an unreserved character according to
        // https://datatracker.ietf.org/doc/html/rfc3986#section-2.3
        // Those are characters that are allowed in a URI but do not have a reserved
        // purpose (e.g. they do not separate the components of the URI)
        return char.IsLetterOrDigit(c) ||
                c == '-' ||
                c == '.' ||
                c == '_' ||
                c == '~';
    }
    void OnLocationChanged(object? sender, LocationChangedEventArgs args)
    {
        // We could just re-render always, but for this component we know the
        // only relevant state change is to the _isActive property.
        var shouldBeActiveNow = ShouldMatch(args.Location);
        if (shouldBeActiveNow != IsActive)
        {
            IsActive = shouldBeActiveNow;
            StateHasChanged();
        }
    }

    class CaseInsensitiveCharComparer : IEqualityComparer<char>
    {
        public bool Equals(char x, char y)
        {
            return char.ToLowerInvariant(x) == char.ToLowerInvariant(y);
        }
        public int GetHashCode(char obj)
        {
            return char.ToLowerInvariant(obj).GetHashCode();
        }
    }
}
