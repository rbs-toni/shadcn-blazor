namespace ShadcnBlazor;
public record ToastHeight
{
    public ToastHeight(string toastId, ToastPosition? position, double height)
    {
        TimeStamp = DateTime.Now;
        ToastId = toastId;
        Position = position;
        Height = height;
    }
    public string ToastId { get; set; }
    public ToastPosition? Position { get; set; }
    public double Height { get; set; }
    public DateTime TimeStamp { get; }
}
