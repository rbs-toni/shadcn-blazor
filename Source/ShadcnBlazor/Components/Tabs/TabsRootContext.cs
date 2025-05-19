namespace ShadcnBlazor;
public class TabsContext
{
    public TabsContext(Tabs tabs)
    {
        _tabs = tabs;
    }
    private readonly Tabs _tabs;
    public string? Value => _tabs.Value;
    public Orientation Orientation => _tabs.Orientation;
    public Direction Direction => _tabs.Direction;
    public ActivationMode ActivationMode => _tabs.ActivationMode;
    public string? BaseId => _tabs.Id;
    public event Action? ContextChanged;
    public TabsList? TabsList { get; protected set; }
    public void ChangeValue(string value)
    {
        _tabs.ChangeValue(value);
    }

    public void SetTabList(TabsList tabs)
    {
        TabsList = tabs;
    }
}
