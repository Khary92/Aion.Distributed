using Grpc.Core;
using Proto.Notifications.Settings;

namespace Core.Server.Communication.Endpoints.Settings;

public class
    SettingsNotificationService : Proto.Notifications.Settings.SettingsNotificationService.
    SettingsNotificationServiceBase
{
    private CancellationToken _cancellationToken;
    private IServerStreamWriter<SettingsNotification>? _responseStream;

    public override async Task SubscribeSettingsNotifications(
        SubscribeRequest request,
        IServerStreamWriter<SettingsNotification> responseStream,
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

    public async Task SendNotificationAsync(SettingsNotification notification)
    {
        if (_responseStream is not null && !_cancellationToken.IsCancellationRequested)
            try
            {
                await _responseStream.WriteAsync(notification, _cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Senden der SettingsNotification: {ex.Message}");
                _responseStream = null;
            }
    }
}