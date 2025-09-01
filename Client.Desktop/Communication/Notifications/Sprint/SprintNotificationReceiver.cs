using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client.Desktop.Lifecycle.Startup.Tasks.Streams;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using Grpc.Core;
using Proto.Notifications.Sprint;

namespace Client.Desktop.Communication.Notifications.Sprint;

public class SprintNotificationReceiver(
    SprintNotificationService.SprintNotificationServiceClient client,
    IMessenger messenger,
    ITraceCollector tracer) : IStreamClient
{
    public async Task StartListening(CancellationToken cancellationToken)
    {
        using var call =
            client.SubscribeSprintNotifications(new SubscribeRequest(), cancellationToken: cancellationToken);

        while (!cancellationToken.IsCancellationRequested)
            try
            {
                await foreach (var notification in call.ResponseStream.ReadAllAsync(cancellationToken))
                    switch (notification.NotificationCase)
                    {
                        case SprintNotification.NotificationOneofCase.SprintCreated:
                        {
                            var notificationSprintCreated = notification.SprintCreated;
                            await tracer.Sprint.Create.NotificationReceived(GetType(),
                                Guid.Parse(notificationSprintCreated.TraceData.TraceId), notificationSprintCreated);

                            Dispatcher.UIThread.Post(() =>
                            {
                                messenger.Send(notificationSprintCreated.ToNewEntityMessage());
                            });
                            break;
                        }
                        case SprintNotification.NotificationOneofCase.SprintActiveStatusSet:
                        {
                            var notificationSprintActiveStatusSet = notification.SprintActiveStatusSet;
                            await tracer.Sprint.ActiveStatus.NotificationReceived(GetType(),
                                Guid.Parse(notificationSprintActiveStatusSet.TraceData.TraceId),
                                notificationSprintActiveStatusSet);

                            Dispatcher.UIThread.Post(() =>
                            {
                                messenger.Send(notificationSprintActiveStatusSet.ToClientNotification());
                            });
                            break;
                        }
                        case SprintNotification.NotificationOneofCase.SprintDataUpdated:
                        {
                            var notificationSprintDataUpdated = notification.SprintDataUpdated;
                            await tracer.Sprint.Update.NotificationReceived(GetType(),
                                Guid.Parse(notificationSprintDataUpdated.TraceData.TraceId),
                                notificationSprintDataUpdated);

                            Dispatcher.UIThread.Post(() =>
                            {
                                messenger.Send(notificationSprintDataUpdated.ToClientNotification());
                            });
                            break;
                        }
                        case SprintNotification.NotificationOneofCase.TicketAddedToActiveSprint:
                        {
                            var notificationTicketAddedToActiveSprint = notification.TicketAddedToActiveSprint;

                            await tracer.Sprint.AddTicketToSprint.NotificationReceived(GetType(),
                                Guid.Parse(notificationTicketAddedToActiveSprint.TraceData.TraceId),
                                notificationTicketAddedToActiveSprint);

                            Dispatcher.UIThread.Post(() =>
                            {
                                messenger.Send(notificationTicketAddedToActiveSprint.ToClientNotification());
                            });
                            break;
                        }
                    }
            }
            catch (Exception ex)
            {
                Console.WriteLine(GetType() + " caused an exception: " + ex);

                if (cancellationToken.IsCancellationRequested)
                    return;

                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            }
    }
}