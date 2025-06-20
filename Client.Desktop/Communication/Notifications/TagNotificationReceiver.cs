using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client.Desktop.Communication.NotificationWrappers;
using Client.Desktop.DTO;
using CommunityToolkit.Mvvm.Messaging;
using Grpc.Core;
using Proto.Notifications.Tag;

namespace Client.Desktop.Communication.Notifications;

public class TagNotificationReceiver(
    TagNotificationService.TagNotificationServiceClient client,
    IMessenger messenger)
{
    public async Task StartListeningAsync(CancellationToken cancellationToken)
    {
        using var call =
            client.SubscribeTagNotifications(new SubscribeRequest(), cancellationToken: cancellationToken);

        try
        {
            await foreach (var notification in call.ResponseStream.ReadAllAsync(cancellationToken))
                switch (notification.NotificationCase)
                {
                    case TagNotification.NotificationOneofCase.TagCreated:
                    {
                        var created = notification.TagCreated;

                        Dispatcher.UIThread.Post(() =>
                        {
                            messenger.Send(new NewTagMessage(new TagDto(Guid.Parse(created.TagId), created.Name,
                                false)));
                        });
                        break;
                    }
                    case TagNotification.NotificationOneofCase.TagUpdated:
                    {
                        Dispatcher.UIThread.Post(() => { messenger.Send(notification.TagUpdated); });
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
            Console.WriteLine($"Error in TagNotificationReceiver: {ex}");
        }
    }
}