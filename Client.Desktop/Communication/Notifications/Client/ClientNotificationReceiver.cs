using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client.Desktop.Lifecycle.Startup.Tasks.Streams;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using Grpc.Core;
using Proto.Notifications.Client;

namespace Client.Desktop.Communication.Notifications.Client;

public class ClientNotificationReceiver(
    ClientNotificationService.ClientNotificationServiceClient client,
    IMessenger messenger,
    ITraceCollector tracer) : IStreamClient
{
    public async Task StartListening(CancellationToken cancellationToken)
    {
        using var call =
            client.SubscribeClientNotifications(new SubscribeRequest(), cancellationToken: cancellationToken);

        while (!cancellationToken.IsCancellationRequested)
            try
            {
                await foreach (var notification in call.ResponseStream.ReadAllAsync(cancellationToken))
                    switch (notification.NotificationCase)
                    {
                        case ClientNotification.NotificationOneofCase.TimeSlotControlCreated:
                        {
                            await tracer.Client.CreateTrackingControl.NotificationReceived(GetType(),
                                Guid.Parse(notification.TimeSlotControlCreated.TraceData.TraceId),
                                notification.TimeSlotControlCreated);
                            
                            Dispatcher.UIThread.Post(() =>
                            {
                                messenger.Send(notification.TimeSlotControlCreated.ToClientNotification());
                            });
                            break;
                        }
                        case ClientNotification.NotificationOneofCase.SprintSelectionChanged:
                        {
                            Dispatcher.UIThread.Post(() =>
                            {
                                messenger.Send(notification.SprintSelectionChanged.ToClientNotification());
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