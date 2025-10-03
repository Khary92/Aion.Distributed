using System;
using System.Threading;
using System.Threading.Tasks;
using Client.Desktop.Communication.Notifications.TimerSettings.Receiver;
using Client.Desktop.Communication.Notifications.TimerSettings.Records;
using Client.Desktop.Lifecycle.Startup.Tasks.Streams;
using Proto.Command.TimerSettings;
using Service.Proto.Shared.Commands.TimerSettings;

namespace Client.Desktop.Communication.Mock.Commands;

public class MockTimerSettingsCommandSender : ITimerSettingsCommandSender, ILocalTimerSettingsNotificationPublisher, IStreamClient
{
    public Task<bool> Send(CreateTimerSettingsCommandProto command)
    {
        // Do nothing. The client does not manage these kinds of events
        throw new NotImplementedException();
    }

    public Task<bool> Send(ChangeSnapshotSaveIntervalCommandProto command)
    {
        // Do nothing. The client does not manage these kinds of events
        throw new NotImplementedException();
    }

    public Task<bool> Send(ChangeDocuTimerSaveIntervalCommandProto command)
    {
        // Do nothing. The client does not manage these kinds of events
        throw new NotImplementedException();
    }

    public event Func<ClientDocuTimerSaveIntervalChangedNotification, Task>? ClientDocuTimerSaveIntervalChangedNotificationReceived;
    public event Func<ClientSnapshotSaveIntervalChangedNotification, Task>? ClientSnapshotSaveIntervalChangedNotificationReceived;
    public Task Publish(ClientDocuTimerSaveIntervalChangedNotification notification)
    {
        // Do nothing. The client does not manage these kinds of events
        throw new NotImplementedException();
    }

    public Task Publish(ClientSnapshotSaveIntervalChangedNotification notification)
    {
        // Do nothing. The client does not manage these kinds of events
        throw new NotImplementedException();
    }

    public Task StartListening(CancellationToken cancellationToken)
    {
        // Do nothing. The client does not manage these kinds of events
        return Task.CompletedTask;
    }
}