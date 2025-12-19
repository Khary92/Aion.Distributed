using System.Collections.Concurrent;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Proto.Notifications.Sprint;
using SubscribeRequest = Proto.Notifications.Sprint.SubscribeRequest;

namespace Core.Server.Communication.Endpoints.Sprint;

public class
    SprintNotificationService : Proto.Notifications.Sprint.SprintNotificationService.SprintNotificationServiceBase
{
    private readonly ConcurrentDictionary<Guid, (IServerStreamWriter<SprintNotification> Stream,
            CancellationToken Token)>
        _clients = new();

    public override async Task SubscribeSprintNotifications(
        SubscribeRequest request,
        IServerStreamWriter<SprintNotification> responseStream,
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

    public async Task SendNotificationAsync(SprintNotification notification)
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