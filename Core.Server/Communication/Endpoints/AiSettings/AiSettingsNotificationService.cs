using Grpc.Core;
using Proto.Notifications.AiSettings;

namespace Core.Server.Communication.Endpoints.AiSettings;

public class AiSettingsNotificationService : Proto.Notifications.AiSettings.AiSettingsNotificationService.
    AiSettingsNotificationServiceBase
{
    private CancellationToken _cancellationToken;
    private IServerStreamWriter<AiSettingsNotification>? _responseStream;

    public override async Task SubscribeAiSettingsNotifications(
        SubscribeRequest request,
        IServerStreamWriter<AiSettingsNotification> responseStream,
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

    public async Task SendNotificationAsync(AiSettingsNotification notification)
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