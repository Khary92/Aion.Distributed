using System;
using System.Threading;
using System.Threading.Tasks;
using Client.Desktop.Communication.Local.LocalEvents.Records;
using Client.Desktop.Communication.Notifications.Client.Records;
using Client.Desktop.Lifecycle.Startup.Tasks.Streams;
using Client.Tracing.Tracing.Tracers;
using Global.Settings;
using Grpc.Core;
using Grpc.Net.Client;
using Proto.Notifications.Client;

namespace Client.Desktop.Communication.Notifications.Client.Receiver;

public class ClientNotificationReceiver(
    IGrpcUrlService grpcUrlBuilder,
    ITraceCollector tracer) : ILocalClientNotificationPublisher, IStreamClient
{
    public event Func<ClientSprintSelectionChangedNotification, Task>? ClientSprintSelectionChangedNotificationReceived;
    public event Func<ClientTrackingControlCreatedNotification, Task>? ClientTrackingControlCreatedNotificationReceived;

    public async Task Publish(ClientTrackingControlCreatedNotification notification)
    {
        if (ClientTrackingControlCreatedNotificationReceived == null)
            throw new InvalidOperationException(
                "TrackingControlCreatedNotification received but no forwarding receiver is set");

        await ClientTrackingControlCreatedNotificationReceived.Invoke(notification);
    }

    public async Task Publish(ClientSprintSelectionChangedNotification notification)
    {
        if (ClientSprintSelectionChangedNotificationReceived == null)
            throw new InvalidOperationException(
                "SprintSelectionChangedNotification received but no forwarding receiver is set");

        await ClientSprintSelectionChangedNotificationReceived.Invoke(notification);
    }

    public async Task StartListening(CancellationToken cancellationToken)
    {
        var attempt = 0;

        while (!cancellationToken.IsCancellationRequested)
            try
            {
                using var channel = GrpcChannel.ForAddress(grpcUrlBuilder.ClientToServerUrl);

                var client = new ClientNotificationService.ClientNotificationServiceClient(channel);
                using var call =
                    client.SubscribeClientNotifications(new SubscribeRequest(), cancellationToken: cancellationToken);

                attempt = 0;

                await foreach (var notification in call.ResponseStream.ReadAllAsync(cancellationToken))
                    await HandleNotificationReceived(notification, cancellationToken);

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

    private async Task HandleNotificationReceived(ClientNotification notification, CancellationToken stoppingToken)
    {
        switch (notification.NotificationCase)
        {
            case ClientNotification.NotificationOneofCase.TimeSlotControlCreated:
            {
                await tracer.Client.CreateTrackingControl.NotificationReceived(GetType(),
                    Guid.Parse(notification.TimeSlotControlCreated.TraceData.TraceId),
                    notification.TimeSlotControlCreated);

                await Publish(notification.TimeSlotControlCreated.ToClientNotification());
                break;
            }
            case ClientNotification.NotificationOneofCase.SprintSelectionChanged:
            {
                await Publish(notification.SprintSelectionChanged.ToClientNotification());
                break;
            }
        }
    }
}