namespace ShadcnBlazor;
public class TabsRootContext
{
    string? _value;
    public event Action? ContextChanged;

    public ActivationMode ActivationMode { get; set; }
    public string? BaseId { get; set; }
    public Direction? Direction { get; set; }
    public Orientation Orientation { get; set; }
    public string? Value
    {
        get => _value;
        private set
        {
            if (_value != value)
            {
                _value = value;
                NotifyContextChanged();
            }
        }
    }

    public void ChangeModelValue(string value)
    {
        Value = value;
    }
    void NotifyContextChanged()
    {
        ContextChanged?.Invoke();
    }
}
