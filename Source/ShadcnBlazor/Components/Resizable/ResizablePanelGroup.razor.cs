using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Linq;

namespace ShadcnBlazor;
public partial class ResizablePanelGroup : IDisposable
{
    double[] _layout = Array.Empty<double>();
    ResizablePanelGroupContext _panelGroupContext;
    Dictionary<string, double> _panelIdToLastNotifiedSizeMap = new();
    Dictionary<string, double> _panelSizeBeforeCollapse = new();
    List<PanelData> panels = new();

    public ResizablePanelGroup()
    {
        Id = Identifier.NewId();
        _panelGroupContext = new(this);
    }

    internal DragState? DragState { get; set; }
    string? ClassValue => new CssBuilder(Class).AddClass(DefaultClasses).Build();
    [Inject]
    IJSRuntime JSRuntime { get; set; } = default!;
    string? StyleValue => new StyleBuilder(Style)
    .AddStyle("display", "flex")
        .AddStyle("flex-direction", Direction == ResizeDirection.Horizontal ? "row" : "column")
        .AddStyle("height", "100%")
        .AddStyle("width", "100%")
        .AddStyle("overflow", "hidden")
        .Build();

    public void CollapsePanel(PanelData panelData)
    {
        if (panelData.Constraints.Collapsible)
        {
            var currentSize = GetPanelSize(panelData);
            if (currentSize != panelData.Constraints.CollapsedSize)
            {
                _panelSizeBeforeCollapse[panelData.Id] = currentSize;
                ResizePanel(panelData, panelData.Constraints.CollapsedSize ?? 0);
            }
        }
    }
    public void ExpandPanel(PanelData panelData)
    {
        if (panelData.Constraints.Collapsible && GetPanelSize(panelData) == panelData.Constraints.CollapsedSize)
        {
            var previousSize = _panelSizeBeforeCollapse.TryGetValue(panelData.Id, out var size)
                ? Math.Max(size, panelData.Constraints.MinSize ?? 0)
                : panelData.Constraints.MinSize ?? 0;
            ResizePanel(panelData, previousSize);
        }
    }
    public double GetPanelSize(PanelData panelData)
    {
        var index = panels.IndexOf(panelData);
        return index >= 0 && index < _layout.Length ? _layout[index] : 0;
    }
    public string GetPanelStyle(PanelData panelData, double? defaultSize)
    {
        var size = GetPanelSize(panelData);
        var flexBasis = size + "%";

        var style = $"flex: 0 0 {flexBasis};";

        if (Direction == ResizeDirection.Horizontal)
        {
            style += "height: 100%;";
        }
        else
        {
            style += "width: 100%;";
        }

        return style;
    }
    public bool IsPanelCollapsed(PanelData panelData)
    {
        return panelData.Constraints.Collapsible &&
               GetPanelSize(panelData) == panelData.Constraints.CollapsedSize;
    }
    public bool IsPanelExpanded(PanelData panelData)
    {
        return !panelData.Constraints.Collapsible ||
               GetPanelSize(panelData) > panelData.Constraints.CollapsedSize;
    }
    public void ReevaluatePanelConstraints(PanelData panelData, PanelConstraints prevConstraints)
    {
        var index = panels.IndexOf(panelData);
        if (index < 0)
        {
            return;
        }

        var currentSize = GetPanelSize(panelData);

        // Handle collapsible state changes
        if (prevConstraints.Collapsible == true &&
            panelData.Constraints.Collapsible == true &&
            currentSize == prevConstraints.CollapsedSize)
        {
            if (prevConstraints.CollapsedSize != panelData.Constraints.CollapsedSize)
            {
                ResizePanel(panelData, panelData.Constraints.CollapsedSize ?? 0);
            }
        }
        else if (currentSize < panelData.Constraints.MinSize)
        {
            ResizePanel(panelData, panelData.Constraints.MinSize ?? 0);
        }
        else if (currentSize > panelData.Constraints.MaxSize)
        {
            ResizePanel(panelData, panelData.Constraints.MaxSize ?? 100);
        }
    }
    public void RegisterPanel(PanelData panelData)
    {
        if (!panels.Any(p => p.Id == panelData.Id))
        {
            panels.Add(panelData);
            panels = panels.OrderBy(p => p.Order ?? 0).ToList();
            RecalculateLayout();
        }
    }
    public Action<ResizeEventData> RegisterResizeHandle(string dragHandleId)
    {
        return async (eventData) => await HandleResize(dragHandleId, eventData);
    }
    public void ResizePanel(PanelData panelData, double size)
    {
        var index = panels.IndexOf(panelData);
        if (index < 0)
        {
            return;
        }

        var newLayout = _layout.ToArray();
        newLayout[index] = size;
        SetLayout(newLayout);
    }
    public async Task StartDragging(string dragHandleId, ResizeEventData eventData)
    {
        var element = await JSRuntime.InvokeAsync<DomRect>("getBoundingClientRect", Ref);

        DragState = new DragState
        {
            DragHandleId = dragHandleId,
            InitialCursorPosition = Direction == ResizeDirection.Horizontal
                ? eventData.ClientX
                : eventData.ClientY,
            InitialLayout = _layout.ToArray(),
            DragHandleRect = new DomRect
            {
                Height = element.Height,
                Width = element.Width,
                X = element.X,
                Y = element.Y
            }
        };
    }
    public async Task StopDragging()
    {
        DragState = null;
        if (!string.IsNullOrEmpty(AutoSaveId))
        {
            await SaveToStorage();
        }
    }
    public void UnregisterPanel(PanelData panelData)
    {
        if (panels.Remove(panelData))
        {
            _panelIdToLastNotifiedSizeMap.Remove(panelData.Id);
            RecalculateLayout();
        }
    }
    double[] AdjustLayoutByDelta(double delta, double[] initialLayout)
    {
        // Implement the actual layout adjustment logic
        // This would include pivot indices calculation and constraints enforcement
        return initialLayout.Select(x => x + delta / initialLayout.Length).ToArray();
    }
    bool AreLayoutsEqual(double[] a, double[] b)
    {
        if (a.Length != b.Length)
        {
            return false;
        }

        for (int i = 0; i < a.Length; i++)
        {
            if (Math.Abs(a[i] - b[i]) > 0.001)
            {
                return false;
            }
        }
        return true;
    }
    double[] CalculateDefaultLayout()
    {
        if (panels.Count == 0)
        {
            return Array.Empty<double>();
        }

        // If we have default sizes specified, use those
        if (panels.All(p => p.Constraints.DefaultSize.HasValue))
        {
            return panels.Select(p => p.Constraints.DefaultSize.Value).ToArray();
        }

        // Otherwise distribute evenly
        double size = 100.0 / panels.Count;
        return panels.Select(_ => size).ToArray();
    }
    string GetGroupStyle()
    {
        return $"display: flex; flex-direction: {(Direction == ResizeDirection.Horizontal ? "row" : "column")}; " +
               "height: 100%; overflow: hidden; width: 100%;";
    }
    async Task HandleDragMove(ResizeEventData eventData)
    {
        if (DragState == null)
        {
            return;
        }

        var currentPosition = Direction == ResizeDirection.Horizontal
            ? eventData.ClientX
            : eventData.ClientY;

        var delta = currentPosition - DragState.InitialCursorPosition;

        // Adjust for RTL if needed
        if (Direction == ResizeDirection.Horizontal && await IsRtl())
        {
            delta = -delta;
        }

        var newLayout = AdjustLayoutByDelta(delta, DragState.InitialLayout);
        SetLayout(newLayout);
    }
    async Task HandleResize(string dragHandleId, ResizeEventData eventData)
    {
        if (DragState == null)
        {
            await StartDragging(dragHandleId, eventData);
        }
        else
        {
            await HandleDragMove(eventData);
        }
    }
    async Task<bool> IsRtl()
    {
        return await JSRuntime.InvokeAsync<bool>("isRtl", Ref);
    }
    void NotifyPanelsOfSizeChange()
    {
        foreach (var panel in panels)
        {
            var newSize = GetPanelSize(panel);
            if (!_panelIdToLastNotifiedSizeMap.TryGetValue(panel.Id, out var lastSize) ||
                Math.Abs(newSize - lastSize) > 0.001)
            {
                _panelIdToLastNotifiedSizeMap[panel.Id] = newSize;
                panel.Callbacks.OnResize.InvokeAsync(new(newSize, lastSize));
            }
        }
    }
    void RecalculateLayout()
    {
        var newLayout = CalculateDefaultLayout();
        if (!AreLayoutsEqual(_layout, newLayout))
        {
            _layout = newLayout;
            OnLayoutChanged.InvokeAsync(_layout);
            NotifyPanelsOfSizeChange();
        }
    }
    async Task SaveToStorage()
    {
        // Implement storage saving logic
    }
    void SetLayout(double[] newLayout)
    {
        if (!AreLayoutsEqual(_layout, newLayout))
        {
            _layout = newLayout;
            OnLayoutChanged.InvokeAsync(_layout);
            NotifyPanelsOfSizeChange();
        }
    }
}
