using System.Collections.Concurrent;
using Grpc.Core;
using Proto.Notifications.WorkDay;

namespace Core.Server.Communication.Endpoints.WorkDay;

public class
    WorkDayNotificationService : Proto.Notifications.WorkDay.WorkDayNotificationService.WorkDayNotificationServiceBase
{
    private readonly ConcurrentDictionary<Guid, (IServerStreamWriter<WorkDayNotification> Stream, CancellationToken
            Token)>
        _clients = new();

    public override async Task SubscribeWorkDayNotifications(
        SubscribeRequest request,
        IServerStreamWriter<WorkDayNotification> responseStream,
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

    public async Task SendNotificationAsync(WorkDayNotification notification)
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