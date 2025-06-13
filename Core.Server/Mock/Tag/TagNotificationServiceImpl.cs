using Grpc.Core;
using Proto.Notifications.Tag;

namespace Service.Server.Mock.Tag;

public class TagNotificationServiceImpl : TagNotificationService.TagNotificationServiceBase
{
    private IServerStreamWriter<TagNotification>? _responseStream;
    private CancellationToken _cancellationToken;

    public override async Task SubscribeTagNotifications(
        SubscribeRequest request,
        IServerStreamWriter<TagNotification> responseStream,
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
            // Client-Verbindung wurde getrennt
        }
        finally
        {
            _responseStream = null;
        }
    }

    public async Task SendNotificationAsync(TagNotification notification)
    {
        if (_responseStream is not null && !_cancellationToken.IsCancellationRequested)
        {
            try
            {
                await _responseStream.WriteAsync(notification, _cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Senden der TagNotification: {ex.Message}");
                _responseStream = null;
            }
        }
    }
}