using Grpc.Core;
using Proto.Notifications.TimerSettings;

namespace Core.Server.Communication.Endpoints.TimerSettings;

public class TimerSettingsNotificationService : Proto.Notifications.TimerSettings.TimerSettingsNotificationService.
    TimerSettingsNotificationServiceBase
{
    private readonly Dictionary<Guid, (IServerStreamWriter<TimerSettingsNotification> Stream, CancellationToken Token)>
        _clients
            = new();

    private readonly object _lock = new();

    public override async Task SubscribeTimerSettingsNotifications(
        SubscribeRequest request,
        IServerStreamWriter<TimerSettingsNotification> responseStream,
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
            // Verbindung des Clients wurde abgebrochen
        }
        finally
        {
            lock (_lock)
            {
                _clients.Remove(clientId);
            }
        }
    }

    public async Task SendNotificationAsync(TimerSettingsNotification notification)
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