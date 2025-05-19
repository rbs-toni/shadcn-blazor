using System;
using System.Linq;

namespace ShadcnBlazor;
public partial class SplitterPanel
{
    bool _isCollapsed;
    PanelGroupContext? _panelGroupContext;
    string? _panelId;
    double _panelSize;

    string? GroupId => PanelGroupContext?.GroupId;
    bool IsCollapsed => _isCollapsed;
    bool IsExpanded => !_isCollapsed;
    PanelData PanelData =>
        new PanelData
        {
            Callbacks = new PanelCallbacks
            {
                OnCollapse = OnCollapse,
                OnExpand = OnExpand,
                OnResize = OnResize
            },
            Constraints = new PanelConstraints
            {
                CollapsedSize = CollapsedSize.HasValue ? Math.Round(CollapsedSize.Value, Constants.Precision) : null,
                Collapsible = Collapsible,
                DefaultSize = DefaultSize,
                MaxSize = MaxSize,
                MinSize = MinSize
            },
            Id = PanelId,
            IdIsFromProps = Id != null,
            Order = Order
        };
    string? PanelId => _panelId;
    double PanelSize => _panelSize;

    public void Collapse()
    {
        PanelGroupContext?.CollapsePanel(PanelData);
    }
    public void Dispose()
    {
        PanelGroupContext?.UnregisterPanel(PanelData);
    }
    public void Expand()
    {
        PanelGroupContext?.ExpandPanel(PanelData);
    }
    public string GetPanelStyle()
    {
        if (PanelGroupContext == null) return string.Empty;
        return PanelGroupContext.GetPanelStyle(PanelData, DefaultSize);
    }
    public double GetSize()
    {
        return PanelGroupContext?.GetPanelSize(PanelData) ?? 0;
    }
    public void Resize(double size)
    {
        PanelGroupContext?.ResizePanel(PanelData, size);
    }
    public void UpdateCollapsedState(bool isCollapsed)
    {
        _isCollapsed = isCollapsed;
        StateHasChanged();
    }
    public void UpdateSize(double size)
    {
        _panelSize = size;
        StateHasChanged();
    }
    protected override void OnInitialized()
    {
        _panelId = Id ?? Identifier.NewId();
        if (PanelGroupContext != null)
        {
            PanelGroupContext.RegisterPanel(PanelData);
        }
    }

    static class Constants
    {
        public const int Precision = 3;
    }
    record PanelSlotProps(bool IsCollapsed, bool IsExpanded);
}
