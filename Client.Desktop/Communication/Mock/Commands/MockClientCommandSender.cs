using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands.Client;
using Client.Desktop.Communication.Commands.Client.Records;
using Client.Desktop.Communication.Local.LocalEvents.Records;
using Client.Desktop.Communication.Notifications.Client.Receiver;
using Client.Desktop.Communication.Notifications.Client.Records;
using Client.Desktop.DataModels;
using Client.Desktop.Lifecycle.Startup.Tasks.Streams;

namespace Client.Desktop.Communication.Mock.Commands;

public class MockClientCommandSender(MockDataService mockDataService)
    : IClientCommandSender, ILocalClientNotificationPublisher, IStreamClient
{
    private readonly TimeSpan _responseDelay = TimeSpan.FromMilliseconds(50);
    private readonly ConcurrentQueue<ClientCreateTrackingControlCommand> _trackingControlQueue = new();

    public Task<bool> Send(ClientCreateTrackingControlCommand command)
    {
        _trackingControlQueue.Enqueue(command);
        return Task.FromResult(true);
    }

    public event Func<ClientSprintSelectionChangedNotification, Task>? ClientSprintSelectionChangedNotificationReceived;
    public event Func<ClientTrackingControlCreatedNotification, Task>? ClientTrackingControlCreatedNotificationReceived;

    public async Task Publish(ClientTrackingControlCreatedNotification notification)
    {
        if (ClientTrackingControlCreatedNotificationReceived is { } handler)
            await handler(notification);
    }

    public Task Publish(ClientSprintSelectionChangedNotification notification)
    {
        throw new NotImplementedException();
    }

    public async Task StartListening(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            while (_trackingControlQueue.TryDequeue(out var command))
            {
                var currentWorkDay = mockDataService.WorkDays.First();
                var newTimeSlot = new TimeSlotClientModel(Guid.NewGuid(), currentWorkDay.WorkDayId, command.TicketId,
                    DateTimeOffset.Now, DateTimeOffset.Now, [], false);
                var newStaticstdata =
                    new StatisticsDataClientModel(Guid.NewGuid(), newTimeSlot.TimeSlotId, [], true, false, false);

                mockDataService.TimeSlots.Add(newTimeSlot);
                mockDataService.StatisticsData.Add(newStaticstdata);

                var notification = new ClientTrackingControlCreatedNotification(
                    newStaticstdata,
                    mockDataService.Tickets.First(t => t.TicketId == command.TicketId),
                    newTimeSlot,
                    Guid.NewGuid()
                );

                await Task.Delay(_responseDelay, cancellationToken);
                await Publish(notification);
            }

            if (_trackingControlQueue.IsEmpty)
                await Task.Delay(50, cancellationToken);
        }
    }
}