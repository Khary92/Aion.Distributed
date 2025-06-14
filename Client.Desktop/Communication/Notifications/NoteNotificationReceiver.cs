using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client.Desktop.Communication.NotificationWrappers;
using CommunityToolkit.Mvvm.Messaging;
using Contract.DTO;
using Grpc.Core;
using Proto.Notifications.Note;

namespace Client.Desktop.Communication.Notifications;

public class NoteNotificationReceiver(
    NoteNotificationService.NoteNotificationServiceClient client,
    IMessenger messenger)
{
    public async Task StartListeningAsync(CancellationToken cancellationToken)
    {
        using var call =
            client.SubscribeNoteNotifications(new SubscribeRequest(), cancellationToken: cancellationToken);

        try
        {
            await foreach (var notification in call.ResponseStream.ReadAllAsync(cancellationToken))
            {
                switch (notification.NotificationCase)
                {
                    case NoteNotification.NotificationOneofCase.NoteCreated:
                    {
                        var noteCreated = notification.NoteCreated;
                        var noteDto = new NoteDto(
                            Guid.Parse(noteCreated.NoteId),
                            noteCreated.Text,
                            Guid.Parse(noteCreated.NoteTypeId),
                            Guid.Parse(noteCreated.TimeSlotId),
                            DateTimeOffset.Parse(noteCreated.TimeStamp)
                        );

                        Dispatcher.UIThread.Post(() => { messenger.Send(new NewNoteMessage(noteDto)); });

                        break;
                    }
                    case NoteNotification.NotificationOneofCase.NoteUpdated:
                    {
                        Dispatcher.UIThread.Post(() => { messenger.Send(notification.NoteUpdated); });
                        break;
                    }
                }
            }
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in notification listener: {ex}");
        }
    }
}