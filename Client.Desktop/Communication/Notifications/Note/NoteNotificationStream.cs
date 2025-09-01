using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client.Desktop.Lifecycle.Startup.Tasks.Streams;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using Grpc.Core;
using Proto.Notifications.Note;

namespace Client.Desktop.Communication.Notifications.Note;

public class NoteNotificationStream(
    NoteNotificationService.NoteNotificationServiceClient client,
    IMessenger messenger,
    ITraceCollector tracer) : IStreamClient
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
                            var notificationNoteCreated = notification.NoteCreated;

                            await tracer.Note.Create.NotificationReceived(GetType(),
                                Guid.Parse(notificationNoteCreated.NoteId), notificationNoteCreated);

                            Dispatcher.UIThread.Post(() =>
                            {
                                messenger.Send(notificationNoteCreated.ToNewEntityMessage());
                            });
                            break;
                        }
                        case NoteNotification.NotificationOneofCase.NoteUpdated:
                        {
                            var notificationNoteUpdated = notification.NoteUpdated;

                            await tracer.Note.Create.NotificationReceived(GetType(),
                                Guid.Parse(notificationNoteUpdated.NoteId), notificationNoteUpdated);

                            Dispatcher.UIThread.Post(() =>
                            {
                                messenger.Send(notificationNoteUpdated.ToClientNotification());
                            });
                            break;
                        }
                    }
            }
            catch (Exception ex)
            {
                Console.WriteLine(GetType() + " caused an exception: " + ex);

                if (cancellationToken.IsCancellationRequested)
                    return;

                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            }
    }
}