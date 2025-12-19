using System.Collections.Concurrent;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Proto.Notifications.Tag;
using SubscribeRequest = Proto.Notifications.Tag.SubscribeRequest;

namespace Core.Server.Communication.Endpoints.Tag;

public class TagNotificationService : Proto.Notifications.Tag.TagNotificationService.TagNotificationServiceBase
{
    private readonly ConcurrentDictionary<Guid, (IServerStreamWriter<TagNotification> Stream,
            CancellationToken Token)>
        _clients = new();

    public override async Task SubscribeTagNotifications(
        SubscribeRequest request,
        IServerStreamWriter<TagNotification> responseStream,
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

    public async Task SendNotificationAsync(TagNotification notification)
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