using Grpc.Core;
using Proto.Notifications.Note;

namespace Core.Server.Communication.Endpoints.Note;

public interface INoteNotificationService
{
    Task SubscribeNoteNotifications(
        SubscribeRequest request,
        IServerStreamWriter<NoteNotification> responseStream,
        ServerCallContext context);

    Task SendNotificationAsync(NoteNotification notification);
}