namespace ShadcnBlazor;
class CountdownTimer : IDisposable
{
    readonly CancellationToken _cancellationToken;
    readonly int _extendedTimeout;
    readonly int _ticksToTimeout;
    Action? _elapsedDelegate;
    bool _isPaused;
    int _percentComplete;
    Func<int, Task>? _tickDelegate;
    PeriodicTimer _timer;

    internal CountdownTimer(int timeout, int extendedTimeout = 0, CancellationToken cancellationToken = default)
    {
        _ticksToTimeout = 100;
        _timer = new PeriodicTimer(TimeSpan.FromMilliseconds(timeout * 10));
        _cancellationToken = cancellationToken;
        _extendedTimeout = extendedTimeout;
    }

    public void Dispose() => _timer.Dispose();
    internal CountdownTimer OnElapsed(Action elapsedDelegate)
    {
        _elapsedDelegate = elapsedDelegate;
        return this;
    }
    internal CountdownTimer OnTick(Func<int, Task> updateProgressDelegate)
    {
        _tickDelegate = updateProgressDelegate;
        return this;
    }
    internal void Pause()
    {
        _isPaused = true;
    }
    internal async Task StartAsync()
    {
        _percentComplete = 0;
        await DoWorkAsync();
    }
    internal async Task UnPause()
    {
        _isPaused = false;
        if (_extendedTimeout > 0)
        {
            _timer?.Dispose();
            _timer = new PeriodicTimer(TimeSpan.FromMilliseconds(_extendedTimeout * 10));
            await StartAsync();
        }
    }
    async Task DoWorkAsync()
    {
        while (await _timer.WaitForNextTickAsync(_cancellationToken) && !_cancellationToken.IsCancellationRequested)
        {

            if (!_isPaused)
            {
                _percentComplete++;
            }
            if (_tickDelegate != null)
            {
                await _tickDelegate(_percentComplete);
            }

            if (_percentComplete == _ticksToTimeout)
            {
                _elapsedDelegate?.Invoke();
            }
        }
    }
}
