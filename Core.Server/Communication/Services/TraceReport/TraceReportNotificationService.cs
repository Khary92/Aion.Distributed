using Grpc.Core;
using Proto.Notifications.TraceReports;

namespace Service.Server.Communication.Services.TraceReport;

public class TraceReportNotificationService : Proto.Notifications.TraceReports.TraceReportNotificationService.TraceReportNotificationServiceBase
{
    private IServerStreamWriter<TraceReportNotification>? _responseStream;
    private CancellationToken _cancellationToken;

    public override async Task SubscribeTraceReportNotifications(
        SubscribeRequest request,
        IServerStreamWriter<TraceReportNotification> responseStream,
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

    public async Task SendNotificationAsync(TraceReportNotification notification)
    {
        if (_responseStream is not null && !_cancellationToken.IsCancellationRequested)
        {
            try
            {
                await _responseStream.WriteAsync(notification, _cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Senden der TraceReportNotification: {ex.Message}");
                _responseStream = null;
            }
        }
    }
}