using System.Collections.Concurrent;
using Grpc.Core;
using Proto.Notifications.TimeSlots;
using Proto.Notifications.TraceReports;
using SubscribeRequest = Proto.Notifications.TimeSlots.SubscribeRequest;

namespace Core.Server.Communication.Endpoints.TimeSlot;

public class
    TimeSlotNotificationService : Proto.Notifications.TimeSlots.TimeSlotNotificationService.
    TimeSlotNotificationServiceBase
{
    private readonly ConcurrentDictionary<Guid, (IServerStreamWriter<TimeSlotNotification> Stream, CancellationToken Token)>
        _clients = new();

    public override async Task SubscribeTimeSlotNotifications(
        SubscribeRequest request,
        IServerStreamWriter<TimeSlotNotification> responseStream,
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

    public async Task SendNotificationAsync(TimeSlotNotification notification)
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