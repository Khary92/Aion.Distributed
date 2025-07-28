using Grpc.Core;
using Proto.Notifications.TimeSlots;

namespace Core.Server.Communication.Endpoints.TimeSlot;

public class
    TimeSlotNotificationService : Proto.Notifications.TimeSlots.TimeSlotNotificationService.
    TimeSlotNotificationServiceBase
{
    private readonly Dictionary<Guid, (IServerStreamWriter<TimeSlotNotification> Stream, CancellationToken Token)>
        _clients
            = new();

    private readonly Lock _lock = new();

    public override async Task SubscribeTimeSlotNotifications(
        SubscribeRequest request,
        IServerStreamWriter<TimeSlotNotification> responseStream,
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

    public async Task SendNotificationAsync(TimeSlotNotification notification)
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