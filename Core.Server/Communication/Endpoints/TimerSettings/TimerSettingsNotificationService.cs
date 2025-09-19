using System.Collections.Concurrent;
using Grpc.Core;
using Proto.Notifications.TimerSettings;
using SubscribeRequest = Proto.Notifications.TimerSettings.SubscribeRequest;

namespace Core.Server.Communication.Endpoints.TimerSettings;

public class TimerSettingsNotificationService : Proto.Notifications.TimerSettings.TimerSettingsNotificationService.
    TimerSettingsNotificationServiceBase
{
    private readonly ConcurrentDictionary<Guid, (IServerStreamWriter<TimerSettingsNotification> Stream,
            CancellationToken Token)>
        _clients = new();

    public override async Task SubscribeTimerSettingsNotifications(
        SubscribeRequest request,
        IServerStreamWriter<TimerSettingsNotification> responseStream,
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

    public async Task SendNotificationAsync(TimerSettingsNotification notification)
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