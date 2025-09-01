using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client.Desktop.Lifecycle.Startup.Tasks.Streams;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using Grpc.Core;
using Proto.Notifications.StatisticsData;
using SubscribeRequest = Proto.Notifications.StatisticsData.SubscribeRequest;

namespace Client.Desktop.Communication.Notifications.StatisticsData;

public class StatisticsDataNotificationReceiver(
    StatisticsDataNotificationService.StatisticsDataNotificationServiceClient client,
    IMessenger messenger,
    ITraceCollector tracer) : IStreamClient
{
    public async Task StartListening(CancellationToken cancellationToken)
    {
        using var call =
            client.SubscribeStatisticsDataNotifications(new SubscribeRequest(), cancellationToken: cancellationToken);

        while (!cancellationToken.IsCancellationRequested)
            try
            {
                await foreach (var notification in call.ResponseStream.ReadAllAsync(cancellationToken))
                    switch (notification.NotificationCase)
                    {
                        case StatisticsDataNotification.NotificationOneofCase.ChangeProductivity:
                        {
                            var changeProductivityNotification = notification.ChangeProductivity;
                            await tracer.Statistics.ChangeProductivity.NotificationReceived(GetType(),
                                Guid.Parse(changeProductivityNotification.TraceData.TraceId),
                                changeProductivityNotification);

                            Dispatcher.UIThread.Post(() =>
                            {
                                messenger.Send(changeProductivityNotification.ToClientNotification());
                            });
                            break;
                        }
                        case StatisticsDataNotification.NotificationOneofCase.ChangeTagSelection:
                        {
                            var changeTagSelectionNotification = notification.ChangeTagSelection;
                            await tracer.Statistics.ChangeTagSelection.NotificationReceived(GetType(),
                                Guid.Parse(changeTagSelectionNotification.TraceData.TraceId),
                                changeTagSelectionNotification);

                            Dispatcher.UIThread.Post(() =>
                            {
                                messenger.Send(changeTagSelectionNotification.ToClientNotification());
                            });
                            break;
                        }
                    }
            }
            catch (Exception ex)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            }
    }
}