using Grpc.Core;
using Proto.Notifications.UseCase;

namespace Service.Server.Communication.UseCase;

public class UseCaseNotificationServiceImpl : UseCaseNotificationService.UseCaseNotificationServiceBase
{
    private IServerStreamWriter<UseCaseNotification>? _responseStream;
    private CancellationToken _cancellationToken;

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
        {
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
}