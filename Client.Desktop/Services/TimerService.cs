using System;
using System.Threading.Tasks;
using System.Timers;
using Client.Desktop.Communication.Local;
using Client.Desktop.Communication.Local.LocalEvents.Publisher;
using Client.Desktop.Communication.Local.LocalEvents.Records;
using Client.Desktop.Communication.Notifications.TimerSettings.Records;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.Requests.Timer;
using Client.Desktop.DataModels;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Lifecycle.Startup.Tasks.Register;
using Timer = System.Timers.Timer;

namespace Client.Desktop.Services;

public class TimerService(IRequestSender requestSender, INotificationPublisherFacade publisherFacade)
    : IDisposable, IMessengerRegistration, IInitializeAsync, IClientTimerNotificationPublisher
{
    private readonly Timer _timer = new(1000);
    private TimerSettingsClientModel? _timerSettings;

    private bool _disposed;

    public InitializationType Type => InitializationType.Service;

    public async Task InitializeAsync()
    {
        _timerSettings = await requestSender.Send(new ClientGetTimerSettingsRequest(Guid.NewGuid()));
        _timer.Elapsed += OnTick;
        _timer.AutoReset = true;
        _timer.Start();
    }

    public void RegisterMessenger()
    {
        publisherFacade.TimerSettings.ClientDocuTimerSaveIntervalChangedNotificationReceived +=
            HandleClientDocuTimerSaveIntervalChangedNotification;
        publisherFacade.TimerSettings.ClientSnapshotSaveIntervalChangedNotificationReceived +=
            HandleClientSnapshotSaveIntervalChangedNotification;
    }

    public void UnregisterMessenger()
    {
        publisherFacade.TimerSettings.ClientDocuTimerSaveIntervalChangedNotificationReceived -=
            HandleClientDocuTimerSaveIntervalChangedNotification;
        publisherFacade.TimerSettings.ClientSnapshotSaveIntervalChangedNotificationReceived -=
            HandleClientSnapshotSaveIntervalChangedNotification;
    }

    private Task HandleClientDocuTimerSaveIntervalChangedNotification(
        ClientDocuTimerSaveIntervalChangedNotification message)
    {
        _timerSettings!.Apply(message);
        return Task.CompletedTask;
    }

    private Task HandleClientSnapshotSaveIntervalChangedNotification(
        ClientSnapshotSaveIntervalChangedNotification message)
    {
        _timerSettings!.Apply(message);
        return Task.CompletedTask;
    }

    private void OnTick(object? sender, ElapsedEventArgs e)
    {
        _ = HandleTickAsync();
    }

    private async Task HandleTickAsync()
    {
        if (_timerSettings == null) return;

        if (_timerSettings.IsDocuSaveIntervalReached())
        {
            if (ClientSaveDocumentationNotificationReceived == null)
            {
                throw new InvalidOperationException(
                    "Ticket data update received but no forwarding receiver is set");
            }

            await ClientSaveDocumentationNotificationReceived
                .Invoke(new ClientSaveDocumentationNotification());
        }

        if (_timerSettings.IsSnapshotIntervalReached())
        {
            if (ClientCreateSnapshotNotificationReceived == null)
            {
                throw new InvalidOperationException(
                    "Ticket data update received but no forwarding receiver is set");
            }

            await ClientCreateSnapshotNotificationReceived
                .Invoke(new ClientCreateSnapshotNotification());
        }
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        _timer.Stop();
        _timer.Elapsed -= OnTick;
        _timer.Dispose();

        UnregisterMessenger();
    }

    public event Func<ClientCreateSnapshotNotification, Task>? ClientCreateSnapshotNotificationReceived;
    public event Func<ClientSaveDocumentationNotification, Task>? ClientSaveDocumentationNotificationReceived;
}