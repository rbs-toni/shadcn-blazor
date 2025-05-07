namespace ShadcnBlazor;
public class CheckboxChangeEventArgs : EventArgs
{
    public bool? Checked { get; set; }
    public bool? Indeterminate { get; set; }
}
