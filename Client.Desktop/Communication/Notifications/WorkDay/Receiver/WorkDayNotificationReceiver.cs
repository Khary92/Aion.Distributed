using System;
using System.Threading;
using System.Threading.Tasks;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Tracing.Tracing.Tracers;
using Global.Settings.UrlResolver;
using Grpc.Core;
using Grpc.Net.Client;
using Proto.Notifications.WorkDay;

namespace Client.Desktop.Communication.Notifications.WorkDay.Receiver;

public class WorkDayNotificationReceiver(
    IGrpcUrlBuilder grpcUrlBuilder,
    ITraceCollector tracer) : ILocalWorkDayNotificationPublisher
{
    public async Task StartListening(CancellationToken cancellationToken)
    {
        var attempt = 0;

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                using var channel = GrpcChannel.ForAddress(grpcUrlBuilder
                    .From(ResolvingServices.Client)
                    .To(ResolvingServices.Server)
                    .BuildAddress());

                var client = new WorkDayNotificationService.WorkDayNotificationServiceClient(channel);
                using var call =
                    client.SubscribeWorkDayNotifications(new SubscribeRequest(), cancellationToken: cancellationToken);

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
    }

    private async Task HandleNotificationReceived(WorkDayNotification notification)
    {
        switch (notification.NotificationCase)
        {
            case WorkDayNotification.NotificationOneofCase.WorkDayCreated:
            {
                var n = notification.WorkDayCreated;

                await tracer.WorkDay.Create.NotificationReceived(
                    GetType(),
                    Guid.Parse(n.TraceData.TraceId),
                    n);


                if (NewWorkDayMessageReceived == null)
                {
                    throw new InvalidOperationException(
                        "Ticket data update received but no forwarding receiver is set");
                }

                await NewWorkDayMessageReceived.Invoke(n.ToNewEntityMessage());
                break;
            }
            case WorkDayNotification.NotificationOneofCase.None:
                break;
        }
    }

    public event Func<NewWorkDayMessage, Task>? NewWorkDayMessageReceived;
}