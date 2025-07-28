using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client.Desktop.Lifecycle.Startup.Streams;
using CommunityToolkit.Mvvm.Messaging;
using Grpc.Core;
using Proto.Notifications.Note;

namespace Client.Desktop.Communication.Notifications.Note;

public class NoteNotificationStream(
    NoteNotificationService.NoteNotificationServiceClient client,
    IMessenger messenger) : IStreamClient
{
    public async Task StartListening(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
            try
            {
                using var call =
                    client.SubscribeNoteNotifications(new SubscribeRequest(), cancellationToken: cancellationToken);

                await foreach (var notification in call.ResponseStream.ReadAllAsync(cancellationToken))
                    switch (notification.NotificationCase)
                    {
                        case NoteNotification.NotificationOneofCase.NoteCreated:
                        {
                            Dispatcher.UIThread.Post(() =>
                            {
                                messenger.Send(notification.NoteCreated.ToNewEntityMessage());
                            });
                            break;
                        }
                        case NoteNotification.NotificationOneofCase.NoteUpdated:
                        {
                            Dispatcher.UIThread.Post(() =>
                            {
                                messenger.Send(notification.NoteUpdated.ToClientNotification());
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