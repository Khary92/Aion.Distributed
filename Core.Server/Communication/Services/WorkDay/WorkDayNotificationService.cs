using Grpc.Core;
using Proto.Notifications.WorkDay;

namespace Service.Server.Communication.Services.WorkDay;

public class WorkDayNotificationService : Proto.Notifications.WorkDay.WorkDayNotificationService.WorkDayNotificationServiceBase
{
    private IServerStreamWriter<WorkDayNotification>? _responseStream;
    private CancellationToken _cancellationToken;

    public override async Task SubscribeWorkDayNotifications(
        SubscribeRequest request,
        IServerStreamWriter<WorkDayNotification> responseStream,
        ServerCallContext context)
    {
        _responseStream = responseStream;
        _cancellationToken = context.CancellationToken;

        try
        {
            await Task.Delay(Timeout.Infinite, _cancellationToken);
        }
        catch (OperationCanceledException)
        {
            // Stream wurde beendet
        }
        finally
        {
            _responseStream = null;
        }
    }

    public async Task SendNotificationAsync(WorkDayNotification notification)
    {
        if (_responseStream != null && !_cancellationToken.IsCancellationRequested)
        {
            try
            {
                await _responseStream.WriteAsync(notification, _cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Senden der WorkDayNotification: {ex.Message}");
                _responseStream = null;
            }
        }
    }
}