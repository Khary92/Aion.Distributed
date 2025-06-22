using Grpc.Core;
using Proto.Notifications.TimeSlots;

namespace Core.Server.Communication.Endpoints.TimeSlot;

public class
    TimeSlotNotificationService : Proto.Notifications.TimeSlots.TimeSlotNotificationService.
    TimeSlotNotificationServiceBase
{
    private CancellationToken _cancellationToken;
    private IServerStreamWriter<TimeSlotNotification>? _responseStream;

    public override async Task SubscribeTimeSlotNotifications(
        SubscribeRequest request,
        IServerStreamWriter<TimeSlotNotification> responseStream,
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
            // Verbindung wurde getrennt
        }
        finally
        {
            _responseStream = null;
        }
    }

    public async Task SendNotificationAsync(TimeSlotNotification notification)
    {
        if (_responseStream is not null && !_cancellationToken.IsCancellationRequested)
            try
            {
                await _responseStream.WriteAsync(notification, _cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Senden der TimeSlotNotification: {ex.Message}");
                _responseStream = null;
            }
    }
}