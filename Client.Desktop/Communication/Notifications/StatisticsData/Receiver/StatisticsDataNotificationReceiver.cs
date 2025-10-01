using System;
using System.Threading;
using System.Threading.Tasks;
using Client.Desktop.Communication.Notifications.StatisticsData.Records;
using Client.Desktop.Lifecycle.Startup.Tasks.Streams;
using Client.Tracing.Tracing.Tracers;
using Global.Settings.UrlResolver;
using Grpc.Core;
using Grpc.Net.Client;
using Proto.Notifications.StatisticsData;
using SubscribeRequest = Proto.Notifications.StatisticsData.SubscribeRequest;

namespace Client.Desktop.Communication.Notifications.StatisticsData.Receiver;

public class StatisticsDataNotificationReceiver(
    IGrpcUrlBuilder grpcUrlBuilder,
    ITraceCollector tracer) : ILocalStatisticsDataNotificationPublisher, IStreamClient
{
    public event Func<ClientChangeProductivityNotification, Task>? ClientChangeProductivityNotificationReceived;
    public event Func<ClientChangeTagSelectionNotification, Task>? ClientChangeTagSelectionNotificationReceived;

    public async Task StartListening(CancellationToken cancellationToken)
    {
        var attempt = 0;

        while (!cancellationToken.IsCancellationRequested)
            try
            {
                using var channel = GrpcChannel.ForAddress(grpcUrlBuilder
                    .From(ResolvingServices.Client)
                    .To(ResolvingServices.Server)
                    .BuildAddress());

                var client = new StatisticsDataNotificationService.StatisticsDataNotificationServiceClient(channel);
                using var call =
                    client.SubscribeStatisticsDataNotifications(new SubscribeRequest(),
                        cancellationToken: cancellationToken);

                attempt = 0;

                await foreach (var notification in call.ResponseStream.ReadAllAsync(cancellationToken))
                    await HandleNotificationReceived(notification);

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled &&
                                          cancellationToken.IsCancellationRequested)
            {
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{GetType()} caused an exception: {ex}");
                if (cancellationToken.IsCancellationRequested)
                    return;

                attempt++;
                var backoffSeconds = Math.Min(30, Math.Pow(2, attempt));
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(backoffSeconds), cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    return;
                }
            }
    }

    private async Task HandleNotificationReceived(StatisticsDataNotification notification)
    {
        switch (notification.NotificationCase)
        {
            case StatisticsDataNotification.NotificationOneofCase.ChangeProductivity:
            {
                var n = notification.ChangeProductivity;
                await tracer.Statistics.ChangeProductivity.NotificationReceived(
                    GetType(),
                    Guid.Parse(n.TraceData.TraceId),
                    n);

                if (ClientChangeProductivityNotificationReceived == null)
                    throw new InvalidOperationException(
                        "Ticket data update received but no forwarding receiver is set");

                await ClientChangeProductivityNotificationReceived.Invoke(n.ToClientNotification());

                break;
            }
            case StatisticsDataNotification.NotificationOneofCase.ChangeTagSelection:
            {
                var n = notification.ChangeTagSelection;
                await tracer.Statistics.ChangeTagSelection.NotificationReceived(
                    GetType(),
                    Guid.Parse(n.TraceData.TraceId),
                    n);

                if (ClientChangeTagSelectionNotificationReceived == null)
                    throw new InvalidOperationException(
                        "Ticket data update received but no forwarding receiver is set");

                await ClientChangeTagSelectionNotificationReceived.Invoke(n.ToClientNotification());

                break;
            }
            case StatisticsDataNotification.NotificationOneofCase.None:
                break;
        }
    }
}