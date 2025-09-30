using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client.Desktop.Communication.Notifications.Sprint.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using Global.Settings.UrlResolver;
using Grpc.Core;
using Grpc.Net.Client;
using Proto.Notifications.Sprint;

namespace Client.Desktop.Communication.Notifications.Sprint.Receiver;

public class SprintNotificationReceiver(
    IGrpcUrlBuilder grpcUrlBuilder,
    IMessenger messenger,
    ITraceCollector tracer) : ILocalSprintNotificationPublisher
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
    }

    private async Task HandleNotificationReceived(SprintNotification notification)
    {
        switch (notification.NotificationCase)
        {
            case SprintNotification.NotificationOneofCase.SprintCreated:
            {
                var n = notification.SprintCreated;
                await tracer.Sprint.Create.NotificationReceived(
                    GetType(),
                    Guid.Parse(n.TraceData.TraceId),
                    n);

                if (NewSprintMessageReceived == null)
                {
                    throw new InvalidOperationException(
                        "Ticket data update received but no forwarding receiver is set");
                }

                await NewSprintMessageReceived.Invoke(n.ToNewEntityMessage());
                
                break;
            }
            case SprintNotification.NotificationOneofCase.SprintActiveStatusSet:
            {
                var n = notification.SprintActiveStatusSet;
                await tracer.Sprint.ActiveStatus.NotificationReceived(
                    GetType(),
                    Guid.Parse(n.TraceData.TraceId),
                    n);

                if (ClientSprintActiveStatusSetNotificationReceived == null)
                {
                    throw new InvalidOperationException(
                        "Ticket data update received but no forwarding receiver is set");
                }

                await ClientSprintActiveStatusSetNotificationReceived.Invoke(n.ToClientNotification());
                
                break;
            }
            case SprintNotification.NotificationOneofCase.SprintDataUpdated:
            {
                var n = notification.SprintDataUpdated;
                await tracer.Sprint.Update.NotificationReceived(
                    GetType(),
                    Guid.Parse(n.TraceData.TraceId),
                    n);

                if (ClientSprintDataUpdatedNotificationReceived == null)
                {
                    throw new InvalidOperationException(
                        "Ticket data update received but no forwarding receiver is set");
                }

                await ClientSprintDataUpdatedNotificationReceived.Invoke(n.ToClientNotification());
                
                break;
            }
            case SprintNotification.NotificationOneofCase.TicketAddedToActiveSprint:
            {
                var n = notification.TicketAddedToActiveSprint;
                await tracer.Sprint.AddTicketToSprint.NotificationReceived(
                    GetType(),
                    Guid.Parse(n.TraceData.TraceId),
                    n);

                if (ClientTicketAddedToActiveSprintNotificationReceived == null)
                {
                    throw new InvalidOperationException(
                        "Ticket data update received but no forwarding receiver is set");
                }

                await ClientTicketAddedToActiveSprintNotificationReceived.Invoke(n.ToClientNotification());
                
                break;
            }
            case SprintNotification.NotificationOneofCase.None:
                break;
        }
    }

    public event Func<ClientSprintActiveStatusSetNotification, Task>? ClientSprintActiveStatusSetNotificationReceived;
    public event Func<ClientSprintDataUpdatedNotification, Task>? ClientSprintDataUpdatedNotificationReceived;
    public event Func<ClientTicketAddedToActiveSprintNotification, Task>? ClientTicketAddedToActiveSprintNotificationReceived;
    public event Func<NewSprintMessage, Task>? NewSprintMessageReceived;
}