using Grpc.Core;
using Proto.Notifications.Ticket;

public class TicketNotificationServiceImpl : TicketNotificationService.TicketNotificationServiceBase
{
    private IServerStreamWriter<TicketNotification>? _responseStream;
    private CancellationToken _cancellationToken;

    public override async Task SubscribeTicketNotifications(
        SubscribeRequest request,
        IServerStreamWriter<TicketNotification> responseStream,
        ServerCallContext context)
    {
        _responseStream = responseStream;
        _cancellationToken = context.CancellationToken;

        try
        {
            // Warten, bis Client abbricht
            await Task.Delay(Timeout.Infinite, context.CancellationToken);
        }
        catch (OperationCanceledException)
        {
            // Verbindung beendet
        }
        finally
        {
            _responseStream = null;
        }
    }

    public async Task SendNotificationAsync(TicketNotification notification)
    {
        if (_responseStream is not null && !_cancellationToken.IsCancellationRequested)
        {
            try
            {
                await _responseStream.WriteAsync(notification);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Senden: {ex.Message}");
                _responseStream = null;
            }
        }
    }
}