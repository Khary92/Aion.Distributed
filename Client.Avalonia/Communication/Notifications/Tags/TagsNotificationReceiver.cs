using System;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using Contract.DTO;
using Grpc.Core;
using Proto.Notifications.Tag;

namespace Client.Avalonia.Communication.Notifications.Tags;

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
            {
                switch (notification.NotificationCase)
                {
                    case TagNotification.NotificationOneofCase.TagCreated:
                    {
                        var created = notification.TagCreated;

                        messenger.Send(new NewTagMessage(new TagDto(Guid.Parse(created.TagId), created.Name, false)));
                        break;
                    }
                    case TagNotification.NotificationOneofCase.TagUpdated:
                    {
                        messenger.Send(notification.TagUpdated);
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
            Console.WriteLine($"Error in TagNotificationReceiver: {ex}");
        }
    }
}