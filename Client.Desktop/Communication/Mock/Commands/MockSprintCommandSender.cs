using System;
using System.Threading;
using System.Threading.Tasks;
using Client.Desktop.Communication.Notifications.Sprint.Receiver;
using Client.Desktop.Communication.Notifications.Sprint.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.Lifecycle.Startup.Tasks.Streams;
using Proto.Command.Sprints;
using Service.Proto.Shared.Commands.Sprints;

namespace Client.Desktop.Communication.Mock.Commands;

public class MockSprintCommandSender : ISprintCommandSender, ILocalSprintNotificationPublisher, IStreamClient
{
    public Task<bool> Send(CreateSprintCommandProto command)
    {
        // Do nothing. The client does not manage these kinds of events
        throw new NotImplementedException();
    }

    public Task<bool> Send(AddTicketToActiveSprintCommandProto command)
    {
        // Do nothing. The client does not manage these kinds of events
        throw new NotImplementedException();
    }

    public Task<bool> Send(SetSprintActiveStatusCommandProto command)
    {
        // Do nothing. The client does not manage these kinds of events
        throw new NotImplementedException();
    }

    public Task<bool> Send(UpdateSprintDataCommandProto command)
    {
        // Do nothing. The client does not manage these kinds of events
        throw new NotImplementedException();
    }

    public event Func<ClientSprintActiveStatusSetNotification, Task>? ClientSprintActiveStatusSetNotificationReceived;
    public event Func<ClientSprintDataUpdatedNotification, Task>? ClientSprintDataUpdatedNotificationReceived;
    public event Func<ClientTicketAddedToActiveSprintNotification, Task>? ClientTicketAddedToActiveSprintNotificationReceived;
    public event Func<NewSprintMessage, Task>? NewSprintMessageReceived;
    public Task Publish(NewSprintMessage message)
    {
        // Do nothing. The client does not manage these kinds of events
        throw new NotImplementedException();
    }

    public Task Publish(ClientSprintActiveStatusSetNotification notification)
    {
        // Do nothing. The client does not manage these kinds of events
        throw new NotImplementedException();
    }

    public Task Publish(ClientSprintDataUpdatedNotification notification)
    {
        // Do nothing. The client does not manage these kinds of events
        throw new NotImplementedException();
    }

    public Task Publish(ClientTicketAddedToActiveSprintNotification notification)
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