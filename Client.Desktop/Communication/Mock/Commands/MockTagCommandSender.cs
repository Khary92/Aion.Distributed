using System;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;
using Client.Desktop.Communication.Notifications.Tag.Receiver;
using Client.Desktop.Communication.Notifications.Tag.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.Lifecycle.Startup.Tasks.Streams;
using Proto.Command.Tags;
using Service.Proto.Shared.Commands.Tags;

namespace Client.Desktop.Communication.Mock.Commands;

public class MockTagCommandSender : ITagCommandSender, ILocalTagNotificationPublisher, IStreamClient
{
    public Task<bool> Send(CreateTagCommandProto command)
    {
        // Do nothing. The client does not manage these kinds of events
        throw new NotImplementedException();
    }

    public Task<bool> Send(UpdateTagCommandProto command)
    {
        // Do nothing. The client does not manage these kinds of events
        throw new NotImplementedException();
    }

    public event Func<ClientTagUpdatedNotification, Task>? ClientTagUpdatedNotificationReceived;
    public event Func<NewTagMessage, Task>? NewTagMessageNotificationReceived;
    public Task Publish(NewTagMessage message)
    {
        // Do nothing. The client does not manage these kinds of events
        throw new NotImplementedException();
    }

    public Task Publish(ClientTagUpdatedNotification notification)
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