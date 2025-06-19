using Grpc.Core;
using Proto.Notifications.NoteType;

namespace Service.Server.Communication.NoteType;

public class NoteTypeNotificationServiceImpl : NoteTypeNotificationService.NoteTypeNotificationServiceBase
{
    private IServerStreamWriter<NoteTypeNotification>? _responseStream;
    private CancellationToken _cancellationToken;

    public override async Task SubscribeNoteNotifications(
        SubscribeRequest request,
        IServerStreamWriter<NoteTypeNotification> responseStream,
        ServerCallContext context)
    {
        _responseStream = responseStream;
        _cancellationToken = context.CancellationToken;

        try
        {
            await Task.Delay(Timeout.Infinite, context.CancellationToken);
        }
        catch (OperationCanceledException)
        {
            // Verbindung wurde getrennt
        }
        finally
        {
            _responseStream = null;
        }
    }

    public async Task SendNotificationAsync(NoteTypeNotification notification)
    {
        if (_responseStream is not null && !_cancellationToken.IsCancellationRequested)
        {
            try
            {
                await _responseStream.WriteAsync(notification, _cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Senden der NoteTypeNotification: {ex.Message}");
                _responseStream = null;
            }
        }
    }
}