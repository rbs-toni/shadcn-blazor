using System.ComponentModel;

namespace ShadcnBlazor.Docs.Enums
{

    /// <summary>
    /// Specifies the relationship between documents (corresponds to HTML link rel attribute)
    /// </summary>
    public enum Rel
    {
        /// <summary>No relationship specified</summary>
        [Description("")]
        None,

        /// <summary>No referrer information should be sent</summary>
        [Description("noreferrer")]
        NoReferrer,

        /// <summary>No referrer information should be sent, and no opener should be set</summary>
        [Description("noreferrer noopener")]
        NoReferrerNoOpener,

        /// <summary>Indicates an alternative representation of the current document</summary>
        [Description("alternate")]
        Alternate,

        /// <summary>Provides a link to a help document</summary>
        [Description("help")]
        Help,

        /// <summary>Prefetch the linked resource</summary>
        [Description("prefetch")]
        Prefetch,

        /// <summary>Indicates that the link should be preconnected to the origin</summary>
        [Description("preconnect")]
        Preconnect
    }
}
