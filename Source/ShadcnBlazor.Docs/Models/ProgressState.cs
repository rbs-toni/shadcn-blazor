namespace ShadcnBlazor.Docs;

/// <summary>
/// Represents the current development status of a component or documentation item.
/// </summary>
public enum ProgressState
{
    /// <summary>
    /// The component has not been implemented or documented yet.
    /// </summary>
    NotStarted,

    /// <summary>
    /// The component is under active development or partially implemented.
    /// </summary>
    InProgress,

    /// <summary>
    /// The component is fully implemented, tested, and documented.
    /// </summary>
    Completed
}
