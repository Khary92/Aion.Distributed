using Grpc.Core;
using Proto.Notifications.Ticket;

namespace Core.Server.Communication.Services.Ticket;

public class
    TicketNotificationService : Proto.Notifications.Ticket.TicketNotificationService.TicketNotificationServiceBase
{
    private CancellationToken _cancellationToken;
    private IServerStreamWriter<TicketNotification>? _responseStream;

    public override async Task SubscribeTicketNotifications(
        SubscribeRequest request,
        IServerStreamWriter<TicketNotification> responseStream,
        ServerCallContext context)
    {
        _responseStream = responseStream;
        _cancellationToken = context.CancellationToken;

        try
        {
            await Task.Delay(Timeout.Infinite, context.CancellationToken);
        }
        catch (OperationCanceledException)
        {
        }
        finally
        {
            _responseStream = null;
        }
    }

    public async Task SendNotificationAsync(TicketNotification notification)
    {
        if (_responseStream is not null && !_cancellationToken.IsCancellationRequested)
            try
            {
                await _responseStream.WriteAsync(notification, _cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Senden: {ex.Message}");
                _responseStream = null;
            }
    }
}