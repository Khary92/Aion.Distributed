using System;
using System.Threading.Tasks;
using System.Timers;
using Client.Desktop.Communication.Notifications.TimerSettings.Records;
using Client.Desktop.Communication.Notifications.UseCase.Records;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.Requests.Timer;
using Client.Desktop.DataModels;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Lifecycle.Startup.Tasks.Register;
using CommunityToolkit.Mvvm.Messaging;
using Timer = System.Timers.Timer;

namespace Client.Desktop.Services;

public class TimerService(IRequestSender requestSender, IMessenger messenger)
    : IRegisterMessenger, IInitializeAsync, ITimerService
{
    private readonly Timer _timer = new Timer(1000);
    private TimerSettingsClientModel? _timerSettings;

    private void OnTick(object? sender, ElapsedEventArgs e)
    {
        if (_timerSettings == null) return;

        if (_timerSettings.IsDocuSaveIntervalReached())
        {
            messenger.Send(new ClientSaveDocumentationNotification());
        }

        if (_timerSettings.IsSnapSveIntervalReached())
        {
            messenger.Send(new ClientCreateSnapshotNotification());
        }
    }

    public void RegisterMessenger()
    {
        messenger.Register<ClientSnapshotSaveIntervalChangedNotification>(this,
            (_, notification) => { _timerSettings!.Apply(notification); });

        messenger.Register<ClientDocuTimerSaveIntervalChangedNotification>(this,
            (_, notification) => { _timerSettings!.Apply(notification); });
    }

    public InitializationType Type => InitializationType.Service;

    public async Task InitializeAsync()
    {
        _timerSettings = await requestSender.Send(new ClientGetTimerSettingsRequest(Guid.NewGuid()));
        _timer.Elapsed += OnTick;
        _timer.AutoReset = true;
        _timer.Start();
    }
}