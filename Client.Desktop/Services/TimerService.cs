using System;
using System.Threading.Tasks;
using System.Timers;
using Client.Desktop.Communication.Notifications.Client.Records;
using Client.Desktop.Communication.Notifications.TimerSettings.Records;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.Requests.Timer;
using Client.Desktop.DataModels;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Lifecycle.Startup.Tasks.Register;
using CommunityToolkit.Mvvm.Messaging;
using Timer = System.Timers.Timer;

namespace Client.Desktop.Services;

public class TimerService(IRequestSender requestSender, IMessenger messenger)
    : IMessengerRegistration, IInitializeAsync, IRecipient<ClientSnapshotSaveIntervalChangedNotification>,
        IRecipient<ClientDocuTimerSaveIntervalChangedNotification>
{
    private readonly Timer _timer = new(1000);
    private TimerSettingsClientModel? _timerSettings;

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
        messenger.RegisterAll(this);
    }

    public void UnregisterMessenger()
    {
        messenger.UnregisterAll(this);
    }

    public void Receive(ClientDocuTimerSaveIntervalChangedNotification message)
    {
        _timerSettings!.Apply(message);
    }

    public void Receive(ClientSnapshotSaveIntervalChangedNotification message)
    {
        _timerSettings!.Apply(message);
    }

    private void OnTick(object? sender, ElapsedEventArgs e)
    {
        if (_timerSettings == null) return;

        if (_timerSettings.IsDocuSaveIntervalReached()) messenger.Send(new ClientSaveDocumentationNotification());

        if (_timerSettings.IsSnapSveIntervalReached()) messenger.Send(new ClientCreateSnapshotNotification());
    }
}