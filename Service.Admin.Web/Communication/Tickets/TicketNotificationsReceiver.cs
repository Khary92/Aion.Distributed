using Grpc.Core;
using Grpc.Net.Client;
using Proto.Notifications.Ticket;
using Service.Admin.Tracing;
using Service.Admin.Web.Communication.Tickets.State;

namespace Service.Admin.Web.Communication.Tickets;

public class TicketNotificationsReceiver(ITraceCollector tracer, ITicketStateService ticketStateService)
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

        var channel = GrpcChannel.ForAddress("http://core-service:8080", channelOptions);
        var client = new TicketNotificationService.TicketNotificationServiceClient(channel);

        while (!stoppingToken.IsCancellationRequested)
            try
            {
                using var call =
                    client.SubscribeTicketNotifications(new SubscribeRequest(), cancellationToken: stoppingToken);

                await foreach (var notification in call.ResponseStream.ReadAllAsync(stoppingToken))
                    switch (notification.NotificationCase)
                    {
                        case TicketNotification.NotificationOneofCase.TicketCreated:
                            var newTicketMessage = notification.TicketCreated.ToNewEntityMessage();

                            await tracer.Ticket.Create.NotificationReceived(GetType(), newTicketMessage.TraceId,
                                newTicketMessage);
                            
                            await ticketStateService.AddTicket(newTicketMessage);
                            break;

                        case TicketNotification.NotificationOneofCase.TicketDataUpdated:
                            var updateNotification = notification.TicketDataUpdated.ToNotification();
                            
                            await tracer.Ticket.Update.NotificationReceived(GetType(), updateNotification.TraceId,
                                updateNotification);
                            
                            await ticketStateService.Apply(notification.TicketDataUpdated.ToNotification());
                            break;

                        case TicketNotification.NotificationOneofCase.TicketDocumentationUpdated:
                            await ticketStateService.Apply(notification.TicketDocumentationUpdated.ToNotification());
                            break;
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
                await Task.Delay(5000, stoppingToken);
            }
    }
}