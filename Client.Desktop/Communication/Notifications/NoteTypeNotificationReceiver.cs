using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client.Desktop.Communication.NotificationWrappers;
using Client.Desktop.DTO;
using CommunityToolkit.Mvvm.Messaging;
using Grpc.Core;
using Proto.Notifications.NoteType;

namespace Client.Desktop.Communication.Notifications;

public class NoteTypeNotificationReceiver(NoteTypeProtoNotificationService.NoteTypeProtoNotificationServiceClient client, IMessenger messenger)
{
    public async Task StartListeningAsync(CancellationToken cancellationToken)
    {
        using var call =
            client.SubscribeNoteNotifications(new SubscribeRequest(), cancellationToken: cancellationToken);

        try
        {
            await foreach (var notification in call.ResponseStream.ReadAllAsync(cancellationToken))
                switch (notification.NotificationCase)
                {
                    case NoteTypeNotification.NotificationOneofCase.NoteTypeColorChanged:
                    {
                        Dispatcher.UIThread.Post(() => { messenger.Send(notification.NoteTypeColorChanged); });

                        break;
                    }
                    case NoteTypeNotification.NotificationOneofCase.NoteTypeCreated:
                    {
                        var created = notification.NoteTypeCreated;

                        Dispatcher.UIThread.Post(() =>
                        {
                            messenger.Send(new NewNoteTypeMessage(new NoteTypeDto(
                                Guid.Parse(created.NoteTypeId),
                                created.Name,
                                created.Color
                            )));
                        });

                        break;
                    }
                    case NoteTypeNotification.NotificationOneofCase.NoteTypeNameChanged:
                    {
                        Dispatcher.UIThread.Post(() => { messenger.Send(notification.NoteTypeNameChanged); });
                        break;
                    }
                }
        }
        catch (OperationCanceledException)
        {
            // Graceful shutdown
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in NoteTypeNotificationReceiver: {ex}");
        }
    }
}