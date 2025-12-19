using Global.Settings;
using Grpc.Core;
using Grpc.Net.Client;
using Proto.Notifications.Ticket;
using Service.Admin.Tracing;
using Service.Admin.Web.Communication.Authentication;
using Service.Admin.Web.Communication.Mapper;
using Service.Admin.Web.Services.State;

namespace Service.Admin.Web.Communication.Receiver;

public class TicketNotificationsReceiver
{
    private readonly ITraceCollector _tracer;
    private readonly ITicketStateService _ticketStateService;
    private readonly JwtService _jwtService;
    private readonly TicketNotificationService.TicketNotificationServiceClient _client;

    public TicketNotificationsReceiver(
        ITraceCollector tracer,
        ITicketStateService ticketStateService,
        IGrpcUrlService grpcUrlBuilder,
        JwtService jwtService)
    {
        _tracer = tracer;
        _ticketStateService = ticketStateService;
        _jwtService = jwtService;

        var channel = GrpcChannel.ForAddress(grpcUrlBuilder.InternalToServerUrl,
            new GrpcChannelOptions
            {
                HttpHandler = new SocketsHttpHandler
                {
                    EnableMultipleHttp2Connections = true,
                    KeepAlivePingDelay = TimeSpan.FromSeconds(60),
                    KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
                    PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan
                },
                Credentials = ChannelCredentials.Insecure,
                UnsafeUseInsecureChannelCallCredentials = true
            });

        _client = new TicketNotificationService.TicketNotificationServiceClient(channel);
    }

    public async Task SubscribeToNotifications(CancellationToken stoppingToken = default)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var call = _client.SubscribeTicketNotifications(
                    new SubscribeRequest(),
                    headers: new Metadata { { "Authorization", $"Bearer {_jwtService.Token}" } },
                    cancellationToken: stoppingToken);

                await foreach (var notification in call.ResponseStream.ReadAllAsync(stoppingToken))
                {
                    switch (notification.NotificationCase)
                    {
                        case TicketNotification.NotificationOneofCase.TicketCreated:
                            var newTicketMessage = notification.TicketCreated.ToNewEntityMessage();
                            await _tracer.Ticket.Create.NotificationReceived(GetType(), newTicketMessage.TraceId, newTicketMessage);
                            await _ticketStateService.AddTicket(newTicketMessage);
                            break;

                        case TicketNotification.NotificationOneofCase.TicketDataUpdated:
                            var updateNotification = notification.TicketDataUpdated.ToNotification();
                            await _tracer.Ticket.Update.NotificationReceived(GetType(), updateNotification.TraceId, updateNotification);
                            await _ticketStateService.Apply(updateNotification);
                            break;

                        case TicketNotification.NotificationOneofCase.TicketDocumentationUpdated:
                            var ticketDocumentationUpdated = notification.TicketDocumentationUpdated.ToNotification();
                            await _tracer.Ticket.ChangeDocumentation.NotificationReceived(GetType(), ticketDocumentationUpdated.TraceId, ticketDocumentationUpdated);
                            await _ticketStateService.Apply(ticketDocumentationUpdated);
                            break;

                        case TicketNotification.NotificationOneofCase.None:
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
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
}
