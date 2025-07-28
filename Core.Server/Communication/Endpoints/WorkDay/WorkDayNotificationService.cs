using Grpc.Core;
using Proto.Notifications.WorkDay;

namespace Core.Server.Communication.Endpoints.WorkDay;

public class
    WorkDayNotificationService : Proto.Notifications.WorkDay.WorkDayNotificationService.WorkDayNotificationServiceBase
{
    private readonly Dictionary<Guid, (IServerStreamWriter<WorkDayNotification> Stream, CancellationToken Token)>
        _clients
            = new();

    private readonly Lock _lock = new();

    public override async Task SubscribeWorkDayNotifications(
        SubscribeRequest request,
        IServerStreamWriter<WorkDayNotification> responseStream,
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

    public async Task SendNotificationAsync(WorkDayNotification notification)
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