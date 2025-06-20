using Grpc.Core;
using Proto.Notifications.TimerSettings;

namespace Core.Server.Communication.Services.TimerSettings;

public class TimerSettingsNotificationService : Proto.Notifications.TimerSettings.TimerSettingsNotificationService.
    TimerSettingsNotificationServiceBase
{
    private CancellationToken _cancellationToken;
    private IServerStreamWriter<TimerSettingsNotification>? _responseStream;

    public override async Task SubscribeTimerSettingsNotifications(
        SubscribeRequest request,
        IServerStreamWriter<TimerSettingsNotification> responseStream,
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
            // Verbindung wurde abgebrochen
        }
        finally
        {
            _responseStream = null;
        }
    }

    public async Task SendNotificationAsync(TimerSettingsNotification notification)
    {
        if (_responseStream is not null && !_cancellationToken.IsCancellationRequested)
            try
            {
                await _responseStream.WriteAsync(notification, _cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Senden der TimerSettingsNotification: {ex.Message}");
                _responseStream = null;
            }
    }
}