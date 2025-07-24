
using Grpc.Core;
using Proto.Notifications.Tag;

namespace Core.Server.Communication.Endpoints.Tag;

public class TagNotificationService : Proto.Notifications.Tag.TagNotificationService.TagNotificationServiceBase
{
    private readonly Dictionary<Guid, (IServerStreamWriter<TagNotification> Stream, CancellationToken Token)> _clients
        = new();
    private readonly object _lock = new();

    public override async Task SubscribeTagNotifications(
        SubscribeRequest request,
        IServerStreamWriter<TagNotification> responseStream,
        ServerCallContext context)
    {
        var clientId = Guid.NewGuid();
        
        lock (_lock)
        {
            _clients.Add(clientId, (responseStream, context.CancellationToken));
        }

        try
        {
            await Task.Delay(Timeout.Infinite, context.CancellationToken);
        }
        catch (OperationCanceledException)
        {
        }
        finally
        {
            lock (_lock)
            {
                _clients.Remove(clientId);
            }
        }
    }

    public async Task SendNotificationAsync(TagNotification notification)
    {
        List<Guid> clientsToRemove = new();
        
        foreach (var (clientId, (stream, token)) in _clients)
        {
            if (token.IsCancellationRequested)
            {
                clientsToRemove.Add(clientId);
                continue;
            }

            try
            {
                await stream.WriteAsync(notification, token);
            }
            catch (Exception ex)
            {
                clientsToRemove.Add(clientId);
            }
        }

        if (clientsToRemove.Count > 0)
        {
            lock (_lock)
            {
                foreach (var clientId in clientsToRemove)
                {
                    _clients.Remove(clientId);
                }
            }
        }
    }
}