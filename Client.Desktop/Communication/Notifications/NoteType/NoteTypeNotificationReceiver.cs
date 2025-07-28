using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client.Desktop.Lifecycle.Startup.Streams;
using CommunityToolkit.Mvvm.Messaging;
using Grpc.Core;
using Proto.Notifications.NoteType;

namespace Client.Desktop.Communication.Notifications.NoteType;

public class NoteTypeNotificationReceiver(
    NoteTypeProtoNotificationService.NoteTypeProtoNotificationServiceClient client,
    IMessenger messenger) : IStreamClient
{
    public async Task StartListening(CancellationToken cancellationToken)
    {
        using var call =
            client.SubscribeNoteNotifications(new SubscribeRequest(), cancellationToken: cancellationToken);

        while (!cancellationToken.IsCancellationRequested)
            try
            {
                await foreach (var notification in call.ResponseStream.ReadAllAsync(cancellationToken))
                    switch (notification.NotificationCase)
                    {
                        case NoteTypeNotification.NotificationOneofCase.NoteTypeColorChanged:
                        {
                            Dispatcher.UIThread.Post(() =>
                            {
                                messenger.Send(notification.NoteTypeColorChanged.ToClientNotification());
                            });
                            break;
                        }
                        case NoteTypeNotification.NotificationOneofCase.NoteTypeCreated:
                        {
                            Dispatcher.UIThread.Post(() =>
                            {
                                messenger.Send(notification.NoteTypeCreated.ToNewEntityMessage());
                            });
                            break;
                        }
                        case NoteTypeNotification.NotificationOneofCase.NoteTypeNameChanged:
                        {
                            Dispatcher.UIThread.Post(() =>
                            {
                                messenger.Send(notification.NoteTypeNameChanged.ToClientNotification());
                            });
                            break;
                        }
                    }
            }
            catch (Exception ex)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            }
    }
}