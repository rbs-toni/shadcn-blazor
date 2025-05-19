using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace ShadcnBlazor;
public class PanelGroupContext
{
    List<PanelData> _panels = new List<PanelData>();

    public Direction Direction { get; set; }
    public DragState? DragState { get; set; }
    public string? GroupId { get; set; }
    public ElementReference PanelGroupElement { get; set; }
    public IReadOnlyCollection<PanelData> Panels => _panels;

    public void CollapsePanel(PanelData panelData)
    {
        // Implementation for collapsing a panel
    }
    public void ExpandPanel(PanelData panelData)
    {
        // Implementation for expanding a panel
    }
    public double GetPanelSize(PanelData panelData)
    {
        // Implementation for getting the size of a panel
        return 0;
    }
    public string GetPanelStyle(PanelData panelData, double? defaultSize)
    {
        // Implementation for getting the style of a panel
        return string.Empty;
    }
    public bool IsPanelCollapsed(PanelData panelData)
    {
        // Implementation for checking if a panel is collapsed
        return false;
    }
    public bool IsPanelExpanded(PanelData panelData)
    {
        // Implementation for checking if a panel is expanded
        return false;
    }
    public void RegisterPanel(PanelData panelData)
    {
        // Implementation for registering a panel
    }
    public ResizeHandler RegisterResizeHandle(string dragHandleId)
    {
        throw new NotImplementedException();
    }
    public void ResizePanel(PanelData panelData, double size)
    {
        _panels.Add(panelData);
        _panels = [.. _panels.OrderBy(p => p.Order)];
    }
    public void StartDragging(string dragHandleId, ResizeEventArgs eventArgs)
    {
        // Implementation for starting a drag operation
    }
    public void StopDragging()
    {

    }
    public void UnregisterPanel(PanelData panelData)
    {

    }
    void reevaluatePanelConstraints(PanelData panelData, PanelConstraints prevConstraints)
    {
        // Implementation for reevaluating panel constraints
    }

    int findPanelDataIndex(PanelData panelData)
    {
        return _panels.FindIndex(p => p.Id == panelData.Id);
    }
}
