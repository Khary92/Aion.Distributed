using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Client.Desktop.Communication.Notifications.Ticket.Receiver;
using Client.Desktop.Communication.Notifications.Ticket.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.Lifecycle.Startup.Tasks.Streams;
using Proto.Command.Tickets;
using Service.Proto.Shared.Commands.Tickets;

namespace Client.Desktop.Communication.Mock.Commands;

public class MockTicketCommandSender
    : ITicketCommandSender, ILocalTicketNotificationPublisher, IStreamClient
{
    private readonly ConcurrentQueue<UpdateTicketDocumentationCommandProto> _updateDocQueue = new();
    private readonly TimeSpan _responseDelay = TimeSpan.FromMilliseconds(50);

    public event Func<ClientTicketDataUpdatedNotification, Task>? TicketDataUpdatedNotificationReceived;

    public event Func<ClientTicketDocumentationUpdatedNotification, Task>?
        TicketDocumentationUpdatedNotificationReceived;

    public event Func<NewTicketMessage, Task>? NewTicketNotificationReceived;

    public Task<bool> Send(CreateTicketCommandProto command)
    {
        // The client does not manage these kinds of events
        throw new NotImplementedException();
    }

    public Task<bool> Send(UpdateTicketDataCommandProto command)
    {
        // The client does not manage these kinds of events
        throw new NotImplementedException();
    }

    public Task<bool> Send(UpdateTicketDocumentationCommandProto command)
    {
        _updateDocQueue.Enqueue(command);
        return Task.FromResult(true);
    }

    public Task Publish(NewTicketMessage message)
    {
        // The client does not manage these kinds of events
        throw new NotImplementedException();
    }

    public Task Publish(ClientTicketDataUpdatedNotification notification)
    {
        // The client does not manage these kinds of events
        throw new NotImplementedException();
    }

    public async Task Publish(ClientTicketDocumentationUpdatedNotification notification)
    {
        if (TicketDocumentationUpdatedNotificationReceived is { } handler)
            await handler(notification);
    }

    public async Task StartListening(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            while (_updateDocQueue.TryDequeue(out var updateDocCmd))
            {
                var notification = new ClientTicketDocumentationUpdatedNotification(
                    Guid.Parse(updateDocCmd.TicketId),
                    updateDocCmd.Documentation,
                    Guid.Parse(updateDocCmd.TraceData.TraceId)
                );

                await Task.Delay(_responseDelay, cancellationToken);
                await Publish(notification);
            }

            if (_updateDocQueue.IsEmpty)
                await Task.Delay(50, cancellationToken);
        }
    }
}