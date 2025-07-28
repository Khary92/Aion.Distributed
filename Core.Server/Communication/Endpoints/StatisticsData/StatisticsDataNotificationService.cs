using Grpc.Core;
using Proto.Notifications.StatisticsData;

namespace Core.Server.Communication.Endpoints.StatisticsData;

public class
    StatisticsDataNotificationService : Proto.Notifications.StatisticsData.StatisticsDataNotificationService.
    StatisticsDataNotificationServiceBase
{
    private readonly Dictionary<Guid, (IServerStreamWriter<StatisticsDataNotification> Stream, CancellationToken Token)>
        _clients
            = new();

    private readonly Lock _lock = new();

    public override async Task SubscribeStatisticsDataNotifications(
        SubscribeRequest request,
        IServerStreamWriter<StatisticsDataNotification> responseStream,
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

    public async Task SendNotificationAsync(StatisticsDataNotification notification)
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
                Console.WriteLine($"Fehler beim Senden an Client {clientId}: {ex.Message}");
                clientsToRemove.Add(clientId);
            }
        }

        if (clientsToRemove.Count > 0)
            lock (_lock)
            {
                foreach (var clientId in clientsToRemove) _clients.Remove(clientId);
            }
    }
}