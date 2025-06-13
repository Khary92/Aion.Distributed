using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Messaging;
using Contract.DTO;
using Grpc.Core;
using Proto.Notifications.NoteType;

namespace Client.Desktop.Communication.Notifications.NoteType;

public class NoteTypeNotificationReceiver(
    NoteTypeNotificationService.NoteTypeNotificationServiceClient client,
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
                            messenger.Send(messenger.Send(new NewNoteTypeMessage(new NoteTypeDto(
                                Guid.Parse(created.NoteTypeId),
                                created.Name,
                                created.Color
                            ))));
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