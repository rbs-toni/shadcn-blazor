using Microsoft.AspNetCore.Components.Web;

namespace ShadcnBlazor;
public class ResizeEventData
{
    public double ClientX { get; set; }
    public double ClientY { get; set; }
    public string? Key { get; set; }
}

public class ResizeEventArgs 
{
    public double ClientX { get; set; }
    public double ClientY { get; set; }
    public string? Key { get; set; }
}
