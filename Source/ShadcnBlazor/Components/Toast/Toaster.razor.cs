using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace ShadcnBlazor;
public partial class Toaster
{
    bool _expanded;
    bool _interacting;

    public List<(ToastPosition?, int)> PossiblePositions
    {
        get
        {
            return [.. Toasts
                .GroupBy(x => x.Position)
                .Select((x, y) => (x.Key, y))];
        }
    }
    public List<ToastInstance> Toasts { get; set; } = [];
    string? ClassValue => new CssBuilder(Class)
            .AddClass("toaster group pointer-events-auto")
            .Build();
    ToastHeight? Front => _heights.FirstOrDefault();
    [Inject]
    NavigationManager NavigationManager { get; set; } = default!;
    string? StyleValue => new StyleBuilder(Style)
        .AddStyle("--front-toast-height", Front?.Height.ToPx())
        .AddStyle("--width", TOAST_WIDTH.ToPx())
        .AddStyle("--offset", Offset.ToPx())
        .AddStyle("--gap", Gap.ToPx())
        .Build();
    [Inject]
    IToastService ToastService { get; set; } = default!;
    Queue<ToastInstance> ToastWaitingQueue { get; set; } = new();

    public void OnRemoveToast(ToastInstance toast)
    {
        InvokeAsync(() =>
        {
            var toastInstance = Toasts.SingleOrDefault(x => x.Id == toast.Id);

            if (toastInstance is not null)
            {
                Toasts.Remove(toastInstance);
                StateHasChanged();
            }

            if (ToastWaitingQueue.Count != 0)
            {
                ShowEnqueuedToast();
            }
        });
    }
    protected override void OnInitialized()
    {
        ToastService.OnShow += ShowToast;
        ToastService.OnClearAll += ClearAll;
        ToastService.OnClearToasts += ClearToasts;
        ToastService.OnClearQueue += ClearQueue;
        ToastService.OnClearQueueToasts += ClearQueueToasts;

        NavigationManager.LocationChanged += ClearToasts;
    }
    void ClearAll()
    {
        InvokeAsync(() =>
        {
            Toasts.Clear();
            StateHasChanged();
        });
    }
    void ClearQueue()
    {
        InvokeAsync(() =>
        {
            ToastWaitingQueue.Clear();
            StateHasChanged();
        });
    }
    void ClearQueueToasts(ToastType toastLevel)
    {
        InvokeAsync(() =>
        {
            ToastWaitingQueue = new(ToastWaitingQueue.Where(x => x.Type != toastLevel));
            StateHasChanged();
        });
    }
    void ClearToasts(ToastType toastLevel)
    {
        InvokeAsync(() =>
        {
            Toasts.RemoveAll(x => x.Component is null && x.Type == toastLevel);
            StateHasChanged();
        });
    }
    void ClearToasts(object? sender, LocationChangedEventArgs args)
    {
        InvokeAsync(() =>
        {
            Toasts.Clear();
            StateHasChanged();

            if (ToastWaitingQueue.Any())
            {
                ShowEnqueuedToast();
            }
        });
    }
    IEnumerable<(ToastInstance, int)> FilteredToasts(ToastPosition? position, int index)
    {
        return Toasts
            .Where(toast =>
                (toast.Position != null && index == 0) ||
                toast.Position == position
            )
            .OrderByDescending(x => x.TimeStamp)
            .Select((toast, i) => (toast, i));
    }
    async Task<Direction> GetDocumentDirectionAsync()
    {
        return await DocumentService.GetDocumentDirectionAsync();
    }
    List<ToastInstance> GetFilteredToasts(ToastPosition? position)
    {
        return Toasts
            .Where(x => x.Position == position)
            .ToList();
    }
    void OnExpanded(bool expanded)
    {
        if (_expanded == expanded || Toasts.Count <= 1)
        {
            return;
        }
        _expanded = expanded;
    }
    void OnInteracting(bool interacting)
    {
        if (_interacting != interacting)
        {
            _interacting = interacting;
        }
    }
    void OnUpdateHeights(List<ToastHeight> heights)
    {
        _heights = heights;
    }
    void ShowEnqueuedToast()
    {
        InvokeAsync(() =>
        {
            var toast = ToastWaitingQueue.Dequeue();

            Toasts.Add(toast);

            StateHasChanged();
        });
    }
    void ShowToast(ToastInstance instance)
    {
        if (instance.Position == null)
        {
            instance.Position = Position;
        }

        InvokeAsync(() =>
        {
            if (Toasts.Count < VisibleToasts)
            {
                Toasts.Add(instance);

                StateHasChanged();
            }
            else
            {
                ToastWaitingQueue.Enqueue(instance);
            }
        });
    }
}
