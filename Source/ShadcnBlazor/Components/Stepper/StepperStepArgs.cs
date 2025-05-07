namespace ShadcnBlazor;

public class StepperStepArgs
{
    internal StepperStepArgs(int index, int active)
    {
        Index = index;
        Active = index == active;
    }

    public int Index { get; }

    public bool Active { get; }
}
