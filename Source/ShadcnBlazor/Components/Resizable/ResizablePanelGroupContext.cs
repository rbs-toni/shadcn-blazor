using System;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace ShadcnBlazor;
public class ResizablePanelGroupContext
{
    private readonly ResizablePanelGroup _panelGroup;

    public ResizablePanelGroupContext(ResizablePanelGroup panelGroup)
    {
        _panelGroup = panelGroup;
        GroupId = Identifier.NewId();
    }

    public ResizeDirection Direction => _panelGroup.Direction;
    public DragState DragState => _panelGroup.DragState;
    public string GroupId { get; set; }
    public ElementReference PanelGroupElement => _panelGroup.Ref;

    // Exposed functions for child components
    public void CollapsePanel(PanelData panelData)
    {
        _panelGroup.CollapsePanel(panelData);
    }
    public void ExpandPanel(PanelData panelData)
    {
        _panelGroup.ExpandPanel(panelData);
    }
    public double GetPanelSize(PanelData panelData)
    {
        return _panelGroup.GetPanelSize(panelData);
    }
    public string GetPanelStyle(PanelData panelData, double? defaultSize)
    {
        return _panelGroup.GetPanelStyle(panelData, defaultSize);
    }
    public bool IsPanelCollapsed(PanelData panelData)
    {
        return _panelGroup.IsPanelCollapsed(panelData);
    }
    public bool IsPanelExpanded(PanelData panelData)
    {
        return _panelGroup.IsPanelExpanded(panelData);
    }
    public void ReevaluatePanelConstraints(PanelData panelData, PanelConstraints prevConstraints)
    {
        _panelGroup.ReevaluatePanelConstraints(panelData, prevConstraints);
    }
    public void RegisterPanel(PanelData panelData)
    {
        _panelGroup.RegisterPanel(panelData);
    }
    public Action<ResizeEventData> RegisterResizeHandle(string dragHandleId)
    {
        return _panelGroup.RegisterResizeHandle(dragHandleId);
    }
    public void ResizePanel(PanelData panelData, double size)
    {
        _panelGroup.ResizePanel(panelData, size);
    }
    public Task StartDragging(string dragHandleId, ResizeEventData eventData)
    {
        return _panelGroup.StartDragging(dragHandleId, eventData);
    }
    public Task StopDragging()
    {
        return _panelGroup.StopDragging();
    }
    public void UnregisterPanel(PanelData panelData)
    {
        _panelGroup.UnregisterPanel(panelData);
    }
}
