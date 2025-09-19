using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client.Desktop.Lifecycle.Startup.Tasks.Streams;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using Grpc.Core;
using Proto.Notifications.Tag;

namespace Client.Desktop.Communication.Notifications.Tag;

public class TagNotificationReceiver(
    TagNotificationService.TagNotificationServiceClient client,
    IMessenger messenger,
    ITraceCollector tracer) : IStreamClient
{
    public async Task StartListening(CancellationToken cancellationToken)
    {
        using var call =
            client.SubscribeTagNotifications(new SubscribeRequest(), cancellationToken: cancellationToken);

        while (!cancellationToken.IsCancellationRequested)
            try
            {
                await foreach (var notification in call.ResponseStream.ReadAllAsync(cancellationToken))
                    switch (notification.NotificationCase)
                    {
                        case TagNotification.NotificationOneofCase.TagCreated:
                        {
                            var notificationTagCreated = notification.TagCreated;

                            await tracer.Tag.Create.NotificationReceived(GetType(),
                                Guid.Parse(notificationTagCreated.TraceData.TraceId), notificationTagCreated);
                            Dispatcher.UIThread.Post(() =>
                            {
                                messenger.Send(notificationTagCreated.ToNewEntityMessage());
                            });
                            break;
                        }
                        case TagNotification.NotificationOneofCase.TagUpdated:
                        {
                            var notificationTagUpdated = notification.TagUpdated;

                            await tracer.Tag.Update.NotificationReceived(GetType(),
                                Guid.Parse(notificationTagUpdated.TraceData.TraceId), notificationTagUpdated);
                            Dispatcher.UIThread.Post(() =>
                            {
                                messenger.Send(notificationTagUpdated.ToClientNotification());
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