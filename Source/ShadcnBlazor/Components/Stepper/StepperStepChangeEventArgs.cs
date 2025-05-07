namespace ShadcnBlazor;
public class StepperStepChangeEventArgs
{
    internal StepperStepChangeEventArgs(int targetIndex, string targetLabel)
    {
        TargetIndex = targetIndex;
        TargetLabel = targetLabel;
    }

    public int TargetIndex { get; }
    public string TargetLabel { get; }
    public bool IsCancelled { get; set; }
}
