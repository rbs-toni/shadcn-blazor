using ShadcnBlazor.Components.Stepper;
using System;
using System.Linq;

namespace ShadcnBlazor;
public class InternalStepperContext
{
    readonly Stepper _stepper;

    public InternalStepperContext(Stepper stepper)
    {
        _stepper = stepper;
    }

    public int Value { get; set; }
    public Orientation Orientation { get; set; }
    public Direction Direction { get; set; }
    public bool Linear { get; set; }
    public ICollection<StepperItem> Items { get; set; }
}

[Flags]
public enum StepperStepStatus
{
    None = 0,
    Previous = 1,
    Current = 2,
    Next = 4,
    All = Previous | Current | Next
}
