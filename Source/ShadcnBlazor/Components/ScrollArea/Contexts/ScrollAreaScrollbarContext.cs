namespace ShadcnBlazor;
public class ScrollAreaScrollbarContext
{
    readonly ScrollAreaScrollbar _scrollbar;
    bool _isHorizontal;

    public ScrollAreaScrollbarContext(ScrollAreaScrollbar scrollbar)
    {
        _scrollbar = scrollbar;
    }

    public event Action? OnStateChanged;

    public bool IsHorizontal
    {
        get => _isHorizontal; set
        {
            if (_isHorizontal != value)
            {
                _isHorizontal = value;
                NotifyStateChanged();
            }
        }
    }
    public Orientation Orientation => _scrollbar.Orientation;
    public void SetIsHorizontal(bool isHorizontal)
    {
        _isHorizontal = isHorizontal;
        NotifyStateChanged();
    }

    void NotifyStateChanged()
    {
        OnStateChanged?.Invoke();
    }
}
