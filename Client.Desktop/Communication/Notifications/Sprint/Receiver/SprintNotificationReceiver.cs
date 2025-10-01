using System;
using System.Threading;
using System.Threading.Tasks;
using Client.Desktop.Communication.Notifications.Sprint.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.Lifecycle.Startup.Tasks.Streams;
using Client.Tracing.Tracing.Tracers;
using Global.Settings.UrlResolver;
using Grpc.Core;
using Grpc.Net.Client;
using Proto.Notifications.Sprint;

namespace Client.Desktop.Communication.Notifications.Sprint.Receiver;

public class SprintNotificationReceiver(
    IGrpcUrlBuilder grpcUrlBuilder,
    ITraceCollector tracer) : ILocalSprintNotificationPublisher, IStreamClient
{
    public event Func<ClientSprintActiveStatusSetNotification, Task>? ClientSprintActiveStatusSetNotificationReceived;
    public event Func<ClientSprintDataUpdatedNotification, Task>? ClientSprintDataUpdatedNotificationReceived;

    public event Func<ClientTicketAddedToActiveSprintNotification, Task>?
        ClientTicketAddedToActiveSprintNotificationReceived;

    public event Func<NewSprintMessage, Task>? NewSprintMessageReceived;

    public async Task Publish(NewSprintMessage message)
    {
        if (NewSprintMessageReceived == null)
            throw new InvalidOperationException(
                "NewSprintMessage received but no forwarding receiver is set");

        await NewSprintMessageReceived.Invoke(message);
    }

    public async Task Publish(ClientSprintActiveStatusSetNotification notification)
    {
        if (ClientSprintActiveStatusSetNotificationReceived == null)
            throw new InvalidOperationException(
                "SprintActiveStatusSetNotification received but no forwarding receiver is set");

        await ClientSprintActiveStatusSetNotificationReceived.Invoke(notification);
    }

    public async Task Publish(ClientSprintDataUpdatedNotification notification)
    {
        if (ClientSprintDataUpdatedNotificationReceived == null)
            throw new InvalidOperationException(
                "SprintDataUpdatedNotification received but no forwarding receiver is set");

        await ClientSprintDataUpdatedNotificationReceived.Invoke(notification);
    }

    public async Task Publish(ClientTicketAddedToActiveSprintNotification notification)
    {
        if (ClientTicketAddedToActiveSprintNotificationReceived == null)
            throw new InvalidOperationException(
                "TicketAddedToActiveSprintNotification received but no forwarding receiver is set");

        await ClientTicketAddedToActiveSprintNotificationReceived.Invoke(notification);
    }

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

                var client = new SprintNotificationService.SprintNotificationServiceClient(channel);
                using var call =
                    client.SubscribeSprintNotifications(new SubscribeRequest(), cancellationToken: cancellationToken);

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

    private async Task HandleNotificationReceived(SprintNotification notification)
    {
        switch (notification.NotificationCase)
        {
            case SprintNotification.NotificationOneofCase.SprintCreated:
            {
                var notificationSprintCreated = notification.SprintCreated;
                await tracer.Sprint.Create.NotificationReceived(
                    GetType(),
                    Guid.Parse(notificationSprintCreated.TraceData.TraceId),
                    notificationSprintCreated);

                await Publish(notificationSprintCreated.ToNewEntityMessage());

                break;
            }
            case SprintNotification.NotificationOneofCase.SprintActiveStatusSet:
            {
                var notificationSprintActiveStatusSet = notification.SprintActiveStatusSet;
                await tracer.Sprint.ActiveStatus.NotificationReceived(
                    GetType(), Guid.Parse(notificationSprintActiveStatusSet.TraceData.TraceId),
                    notificationSprintActiveStatusSet);

                await Publish(notificationSprintActiveStatusSet.ToClientNotification());

                break;
            }
            case SprintNotification.NotificationOneofCase.SprintDataUpdated:
            {
                var n = notification.SprintDataUpdated;
                await tracer.Sprint.Update.NotificationReceived(
                    GetType(),
                    Guid.Parse(n.TraceData.TraceId),
                    n);

                await Publish(n.ToClientNotification());

                break;
            }
            case SprintNotification.NotificationOneofCase.TicketAddedToActiveSprint:
            {
                var notificationTicketAddedToActiveSprint = notification.TicketAddedToActiveSprint;
                await tracer.Sprint.AddTicketToSprint.NotificationReceived(
                    GetType(),
                    Guid.Parse(notificationTicketAddedToActiveSprint.TraceData.TraceId),
                    notificationTicketAddedToActiveSprint);

                await Publish(notificationTicketAddedToActiveSprint.ToClientNotification());

                break;
            }
            case SprintNotification.NotificationOneofCase.None:
                break;
        }
    }
}