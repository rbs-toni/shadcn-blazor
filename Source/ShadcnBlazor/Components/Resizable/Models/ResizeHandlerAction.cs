using System;
using System.Linq;

namespace ShadcnBlazor;
public enum ResizeHandlerAction
{
    Down,
    Move,
    Up
}

public enum ResizeHandleState
{
    Inactive,
    Hover,
    Drag
}

public delegate void ResizeHandler(ResizeEventArgs args);
