using Grpc.Core;
using Proto.Notifications.Tag;

namespace Service.Server.Communication.Services.Tag;

public class TagNotificationServiceImpl : TagNotificationService.TagNotificationServiceBase
{
    private readonly object _lock = new();
    private IServerStreamWriter<TagNotification>? _responseStream;
    private CancellationToken _cancellationToken = CancellationToken.None;
    private bool _clientConnected = false;

    public override async Task SubscribeTagNotifications(
        SubscribeRequest request,
        IServerStreamWriter<TagNotification> responseStream,
        ServerCallContext context)
    {
        lock (_lock)
        {
            _responseStream = responseStream;
            _cancellationToken = context.CancellationToken;
            _clientConnected = true;
        }

        try
        {
            // Warte bis Client disconnectet
            await Task.Delay(Timeout.Infinite, context.CancellationToken);
        }
        catch (OperationCanceledException)
        {
            // Client-Verbindung wurde getrennt
        }
        finally
        {
            lock (_lock)
            {
                _responseStream = null;
                _clientConnected = false;
            }
        }
    }

    public async Task SendNotificationAsync(TagNotification notification)
    {
        IServerStreamWriter<TagNotification>? streamCopy;
        CancellationToken tokenCopy;

        lock (_lock)
        {
            if (!_clientConnected || _responseStream == null || _cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("SendNotificationAsync: Kein aktiver Client-Stream oder Verbindung beendet.");
                return;
            }

            streamCopy = _responseStream;
            tokenCopy = _cancellationToken;
        }

        try
        {
            await streamCopy.WriteAsync(notification, tokenCopy);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fehler beim Senden der TagNotification: {ex.Message}");

            lock (_lock)
            {
                _responseStream = null;
                _clientConnected = false;
            }
        }
    }
}
