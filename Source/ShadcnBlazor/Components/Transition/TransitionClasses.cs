using System;
using System.Linq;

namespace ShadcnBlazor;
public record TransitionClasses
{
    public string? From { get; set; }
    public string? Active { get; set; }
    public string? To { get; set; }

    /// <summary>
    /// Default enter transition for fade and scale effects.
    /// </summary>
    public static TransitionClasses OnEnterDefaultClasses()
    {
        return new TransitionClasses
        {
            From = "opacity-0 scale-95",
            Active = "transition ease-out duration-5000",
            To = "opacity-100 scale-100"
        };
    }

    /// <summary>
    /// Default leave transition for fade and scale effects.
    /// </summary>
    public static TransitionClasses OnLeaveDefaultClasses()
    {
        return new TransitionClasses
        {
            From = "opacity-100 scale-100",
            Active = "transition ease-in duration-5000",
            To = "opacity-0 scale-95"
        };
    }
}
