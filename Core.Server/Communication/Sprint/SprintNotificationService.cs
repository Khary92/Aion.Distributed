using Grpc.Core;
using Proto.Notifications.Sprint;

namespace Service.Server.Communication.Sprint;

public class SprintNotificationService : Proto.Notifications.Sprint.SprintNotificationService.SprintNotificationServiceBase
{
    private IServerStreamWriter<SprintNotification>? _responseStream;
    private CancellationToken _cancellationToken;

    public override async Task SubscribeSprintNotifications(
        SubscribeRequest request,
        IServerStreamWriter<SprintNotification> responseStream,
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

    public async Task SendNotificationAsync(SprintNotification notification)
    {
        if (_responseStream is not null && !_cancellationToken.IsCancellationRequested)
        {
            try
            {
                await _responseStream.WriteAsync(notification, _cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Senden der SprintNotification: {ex.Message}");
                _responseStream = null;
            }
        }
    }
}