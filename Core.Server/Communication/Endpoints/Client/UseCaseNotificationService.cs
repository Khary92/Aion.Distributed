using System.Collections.Concurrent;
using Grpc.Core;
using Proto.Notifications.Client;

namespace Core.Server.Communication.Endpoints.Client;

public class
    UseCaseNotificationService : ClientNotificationService.ClientNotificationServiceBase
{
    private readonly ConcurrentDictionary<Guid, (IServerStreamWriter<ClientNotification> Stream, CancellationToken
            Token)>
        _clients = new();

    public override async Task SubscribeClientNotifications(
        SubscribeRequest request,
        IServerStreamWriter<ClientNotification> responseStream,
        ServerCallContext context)
    {
        var clientId = Guid.NewGuid();
        _clients[clientId] = (responseStream, context.CancellationToken);

        try
        {
            await Task.Delay(Timeout.Infinite, context.CancellationToken);
        }
        catch (OperationCanceledException)
        {
        }
        finally
        {
            _clients.TryRemove(clientId, out _);
        }
    }

    public async Task SendNotificationAsync(ClientNotification notification)
    {
        foreach (var clientDictionary in _clients)
        {
            var clientId = clientDictionary.Key;
            var (stream, token) = clientDictionary.Value;

            if (token.IsCancellationRequested)
            {
                _clients.TryRemove(clientId, out _);
                continue;
            }

            try
            {
                await stream.WriteAsync(notification, token);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error when sending to {clientId}: {ex.Message}");
                _clients.TryRemove(clientId, out _);
            }
        }
    }
}