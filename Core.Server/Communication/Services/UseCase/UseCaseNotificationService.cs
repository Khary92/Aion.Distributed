using Grpc.Core;
using Proto.Notifications.UseCase;

namespace Core.Server.Communication.Services.UseCase;

public class
    UseCaseNotificationService : Proto.Notifications.UseCase.UseCaseNotificationService.UseCaseNotificationServiceBase
{
    private CancellationToken _cancellationToken;
    private IServerStreamWriter<UseCaseNotification>? _responseStream;

    public override async Task SubscribeUseCaseNotifications(
        SubscribeRequest request,
        IServerStreamWriter<UseCaseNotification> responseStream,
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

    public async Task SendNotificationAsync(UseCaseNotification notification)
    {
        if (_responseStream is not null && !_cancellationToken.IsCancellationRequested)
            try
            {
                await _responseStream.WriteAsync(notification, _cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Senden der UseCaseNotification: {ex.Message}");
                _responseStream = null;
            }
    }
}