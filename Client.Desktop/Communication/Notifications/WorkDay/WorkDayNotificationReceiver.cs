using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client.Desktop.Lifecycle.Startup.Tasks.Streams;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using Grpc.Core;
using Proto.Notifications.WorkDay;

namespace Client.Desktop.Communication.Notifications.WorkDay;

public class WorkDayNotificationReceiver(
    WorkDayNotificationService.WorkDayNotificationServiceClient client,
    IMessenger messenger,
    ITraceCollector tracer) : IStreamClient
{
    public async Task StartListening(CancellationToken cancellationToken)
    {
        using var call =
            client.SubscribeWorkDayNotifications(new SubscribeRequest(), cancellationToken: cancellationToken);

        while (!cancellationToken.IsCancellationRequested)
            try
            {
                await foreach (var notification in call.ResponseStream.ReadAllAsync(cancellationToken))
                    switch (notification.NotificationCase)
                    {
                        case WorkDayNotification.NotificationOneofCase.WorkDayCreated:
                            await tracer.WorkDay.Create.NotificationReceived(GetType(),
                                Guid.Parse(notification.WorkDayCreated.TraceData.TraceId), notification.WorkDayCreated);

                            Dispatcher.UIThread.Post(() =>
                            {
                                messenger.Send(notification.WorkDayCreated.ToNewEntityMessage());
                            });

                            break;
                        case WorkDayNotification.NotificationOneofCase.None:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
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