using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client.Desktop.Lifecycle.Startup.Tasks.Streams;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using Grpc.Core;
using Proto.Notifications.NoteType;

namespace Client.Desktop.Communication.Notifications.NoteType;

public class NoteTypeNotificationReceiver(
    NoteTypeProtoNotificationService.NoteTypeProtoNotificationServiceClient client,
    IMessenger messenger,
    ITraceCollector tracer) : IStreamClient
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
                            var notificationNoteTypeColorChanged = notification.NoteTypeColorChanged;

                            await tracer.NoteType.ChangeColor.NotificationReceived(GetType(),
                                Guid.Parse(notificationNoteTypeColorChanged.NoteTypeId),
                                notificationNoteTypeColorChanged);

                            Dispatcher.UIThread.Post(() =>
                            {
                                messenger.Send(notificationNoteTypeColorChanged.ToClientNotification());
                            });
                            break;
                        }
                        case NoteTypeNotification.NotificationOneofCase.NoteTypeCreated:
                        {
                            var notificationNoteTypeCreated = notification.NoteTypeCreated;

                            await tracer.NoteType.Create.NotificationReceived(GetType(),
                                Guid.Parse(notificationNoteTypeCreated.NoteTypeId),
                                notificationNoteTypeCreated);

                            Dispatcher.UIThread.Post(() =>
                            {
                                messenger.Send(notificationNoteTypeCreated.ToNewEntityMessage());
                            });
                            break;
                        }
                        case NoteTypeNotification.NotificationOneofCase.NoteTypeNameChanged:
                        {
                            var notificationNoteTypeNameChanged = notification.NoteTypeNameChanged;

                            await tracer.NoteType.ChangeName.NotificationReceived(GetType(),
                                Guid.Parse(notificationNoteTypeNameChanged.NoteTypeId),
                                notificationNoteTypeNameChanged);

                            Dispatcher.UIThread.Post(() =>
                            {
                                messenger.Send(notificationNoteTypeNameChanged.ToClientNotification());
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