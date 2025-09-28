using Grpc.Core;
using Grpc.Net.Client;
using Proto.Notifications.Sprint;
using Service.Admin.Tracing;
using Service.Admin.Web.Communication.Mapper;
using Service.Admin.Web.Services.State;
using SubscribeRequest = Proto.Notifications.Sprint.SubscribeRequest;

namespace Service.Admin.Web.Communication.Receiver;

public class SprintNotificationsReceiver(ISprintStateService sprintStateService, ITraceCollector tracer)
{
    public async Task SubscribeToNotifications(CancellationToken stoppingToken = default)
    {
        var channelOptions = new GrpcChannelOptions
        {
            HttpHandler = new SocketsHttpHandler
            {
                EnableMultipleHttp2Connections = true,
                KeepAlivePingDelay = TimeSpan.FromSeconds(60),
                KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
                PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan
            }
        };

        var channel = GrpcChannel.ForAddress("https://core-service:5001", channelOptions);
        var client = new SprintNotificationService.SprintNotificationServiceClient(channel);

        while (!stoppingToken.IsCancellationRequested)
            try
            {
                using var call =
                    client.SubscribeSprintNotifications(new SubscribeRequest(), cancellationToken: stoppingToken);

                await foreach (var notification in call.ResponseStream.ReadAllAsync(stoppingToken))
                    switch (notification.NotificationCase)
                    {
                        case SprintNotification.NotificationOneofCase.SprintCreated:
                            var newSprintMessage = notification.SprintCreated.ToWebModel();

                            await tracer.Sprint.Create.NotificationReceived(GetType(), newSprintMessage.TraceId,
                                notification);

                            await sprintStateService.AddSprint(newSprintMessage);
                            break;

                        case SprintNotification.NotificationOneofCase.SprintDataUpdated:
                            var webSprintDataUpdatedNotification = notification.SprintDataUpdated.ToNotification();

                            await tracer.Sprint.Update.NotificationReceived(GetType(),
                                webSprintDataUpdatedNotification.TraceId, notification);

                            await sprintStateService.Apply(webSprintDataUpdatedNotification);
                            break;

                        case SprintNotification.NotificationOneofCase.SprintActiveStatusSet:
                            var webSetSprintActiveStatusNotification =
                                notification.SprintActiveStatusSet.ToNotification();

                            await tracer.Sprint.ActiveStatus.NotificationReceived(GetType(),
                                webSetSprintActiveStatusNotification.TraceId, notification);

                            await sprintStateService.Apply(webSetSprintActiveStatusNotification);
                            break;

                        case SprintNotification.NotificationOneofCase.TicketAddedToActiveSprint:
                            var webTicketAddedToActiveSprintNotification =
                                notification.TicketAddedToActiveSprint.ToNotification();

                            await tracer.Sprint.AddTicketToSprint.NotificationReceived(GetType(),
                                webTicketAddedToActiveSprintNotification.TraceId, notification);

                            await sprintStateService.Apply(notification.TicketAddedToActiveSprint.ToNotification());
                            break;
                        case SprintNotification.NotificationOneofCase.None:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Unavailable)
            {
                await Task.Delay(5000, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine(GetType() + " caused an error: " + ex);
                await Task.Delay(5000, stoppingToken);
            }
    }
}