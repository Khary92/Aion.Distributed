using System;
using Avalonia.Threading;

namespace Client.Desktop.Views.Custom;

public class ViewTimer
{
    private readonly DispatcherTimer _timer;

    public ViewTimer()
    {
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };

        _timer.Tick += OnTick;
    }

    public event EventHandler? Tick;

    private void OnTick(object? sender, EventArgs e)
    {
        Tick?.Invoke(this, EventArgs.Empty);
    }

    public void Start()
    {
        if (!_timer.IsEnabled)
            _timer.Start();
    }
}