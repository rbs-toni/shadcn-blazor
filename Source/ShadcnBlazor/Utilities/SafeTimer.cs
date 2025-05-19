using System.Threading;
using System.Threading.Tasks;

namespace ShadcnBlazor;
public sealed class SafeTimer : IDisposable
{
    CancellationTokenSource? _cts;
    bool _disposed;
    Timer? _timer;

    public event EventHandler? Elapsed;

    public bool IsRunning { get; private set; }

    public void Dispose()
    {
        if (_disposed)
            return;

        _disposed = true;
        IsRunning = false;
        _cts?.Cancel();
        _timer?.Dispose();
        _cts?.Dispose();
        Elapsed = null;
    }
    public void SetInterval(int interval, Action? callback = null)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(SafeTimer));

        if (interval < 0)
            throw new ArgumentOutOfRangeException(nameof(interval));

        Stop();

        _timer = new Timer(_ =>
        {
            if (!_disposed && IsRunning)
            {
                Elapsed?.Invoke(this, EventArgs.Empty);
                callback?.Invoke();
            }
        }, null, interval, interval);

        IsRunning = true;
    }
    public void SetInterval(int interval, Func<Task>? callback = null)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(SafeTimer));

        if (interval < 0)
            throw new ArgumentOutOfRangeException(nameof(interval));

        Stop();

        _timer = new Timer(async _ =>
        {
            if (!_disposed && IsRunning)
            {
                Elapsed?.Invoke(this, EventArgs.Empty);
                if (callback != null)
                {
                    await callback();
                }
            }
        }, null, interval, interval);

        IsRunning = true;
    }
    public void SetTimeout(int delay, Action? callback = null)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(SafeTimer));

        if (delay < 0)
            throw new ArgumentOutOfRangeException(nameof(delay));

        Stop();

        _timer = new Timer(_ =>
        {
            if (!_disposed && IsRunning)
            {
                IsRunning = false;
                Elapsed?.Invoke(this, EventArgs.Empty);
                callback?.Invoke();
            }
        }, null, delay, Timeout.Infinite);

        IsRunning = true;
    }
    public async Task SetTimeoutAsync(int delay, Func<Task>? callback = null)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(SafeTimer));

        if (delay < 0)
            throw new ArgumentOutOfRangeException(nameof(delay));

        Stop();

        _cts = new CancellationTokenSource();
        IsRunning = true;

        try
        {
            await Task.Delay(delay, _cts.Token);
            if (!_disposed && IsRunning)
            {
                if (callback != null)
                {
                    await callback();
                }
                else
                {
                    Elapsed?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        catch (TaskCanceledException)
        {
            // Normal behavior on cancellation
        }
        finally
        {
            IsRunning = false;
        }
    }
    public void Stop()
    {
        if (_disposed)
            return;

        IsRunning = false;
        _cts?.Cancel();
        _timer?.Change(Timeout.Infinite, Timeout.Infinite);
    }
}
