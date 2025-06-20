using Grpc.Core;
using Proto.Notifications.Note;

namespace Service.Server.Communication.Note;

public class NoteNotificationService : Proto.Notifications.Note.NoteNotificationService.NoteNotificationServiceBase
{
    private IServerStreamWriter<NoteNotification>? _responseStream;
    private CancellationToken _cancellationToken;

    public override async Task SubscribeNoteNotifications(
        SubscribeRequest request,
        IServerStreamWriter<NoteNotification> responseStream,
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
            // Client disconnected
        }
        finally
        {
            _responseStream = null;
        }
    }

    public async Task SendNotificationAsync(NoteNotification notification)
    {
        if (_responseStream is not null && !_cancellationToken.IsCancellationRequested)
        {
            try
            {
                await _responseStream.WriteAsync(notification, _cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Senden der NoteNotification: {ex.Message}");
                _responseStream = null;
            }
        }
    }
}