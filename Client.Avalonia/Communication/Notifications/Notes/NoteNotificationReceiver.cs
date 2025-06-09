using System;
using System.Threading;
using System.Threading.Tasks;
using Client.Avalonia.Communication.NotificationProcessors.Messages;
using CommunityToolkit.Mvvm.Messaging;
using Contract.DTO;
using Grpc.Core;
using Proto.Notifications.Note;

namespace Client.Avalonia.Communication.Notifications;

public class NoteNotificationReceiver
{
    private readonly NoteNotificationService.NoteNotificationServiceClient _client;
    private readonly IMessenger _messenger;

    public NoteNotificationReceiver(NoteNotificationService.NoteNotificationServiceClient client, IMessenger messenger)
    {
        _client = client;
        _messenger = messenger;
    }

    public async Task StartListeningAsync(CancellationToken cancellationToken)
    {
        using var call =
            _client.SubscribeNoteNotifications(new SubscribeRequest(), cancellationToken: cancellationToken);

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
                        _messenger.Send(new NewNoteMessage(noteDto));
                        break;
                    }
                    case NoteNotification.NotificationOneofCase.NoteUpdated:
                    {
                        _messenger.Send(notification.NoteUpdated);
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