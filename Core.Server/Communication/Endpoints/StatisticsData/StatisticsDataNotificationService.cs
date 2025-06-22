using Grpc.Core;
using Proto.Notifications.StatisticsData;

namespace Core.Server.Communication.Endpoints.StatisticsData;

public class
    StatisticsDataNotificationService : Proto.Notifications.StatisticsData.StatisticsDataNotificationService.
    StatisticsDataNotificationServiceBase
{
    private CancellationToken _cancellationToken;
    private IServerStreamWriter<StatisticsDataNotification>? _responseStream;

    public override async Task SubscribeStatisticsDataNotifications(
        SubscribeRequest request,
        IServerStreamWriter<StatisticsDataNotification> responseStream,
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
            // Verbindung beendet
        }
        finally
        {
            _responseStream = null;
        }
    }

    public async Task SendNotificationAsync(StatisticsDataNotification notification)
    {
        if (_responseStream is not null && !_cancellationToken.IsCancellationRequested)
            try
            {
                await _responseStream.WriteAsync(notification, _cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Senden der StatisticsDataNotification: {ex.Message}");
                _responseStream = null;
            }
    }
}