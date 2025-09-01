using System.Collections.Concurrent;
using Grpc.Core;
using Proto.Notifications.StatisticsData;
using SubscribeRequest = Proto.Notifications.StatisticsData.SubscribeRequest;

namespace Core.Server.Communication.Endpoints.StatisticsData;

public class
    StatisticsDataNotificationService : Proto.Notifications.StatisticsData.StatisticsDataNotificationService.
    StatisticsDataNotificationServiceBase
{
    private readonly ConcurrentDictionary<Guid, (IServerStreamWriter<StatisticsDataNotification> Stream,
            CancellationToken Token)>
        _clients = new();

    public override async Task SubscribeStatisticsDataNotifications(
        SubscribeRequest request,
        IServerStreamWriter<StatisticsDataNotification> responseStream,
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

    public async Task SendNotificationAsync(StatisticsDataNotification notification)
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