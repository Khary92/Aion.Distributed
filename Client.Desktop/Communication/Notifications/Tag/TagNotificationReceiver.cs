using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client.Desktop.Lifecycle.Startup.Streams;
using CommunityToolkit.Mvvm.Messaging;
using Grpc.Core;
using Proto.Notifications.Tag;

namespace Client.Desktop.Communication.Notifications.Tag;

public class TagNotificationReceiver(
    TagNotificationService.TagNotificationServiceClient client,
    IMessenger messenger) : IStreamClient
{
    public async Task StartListening(CancellationToken cancellationToken)
    {
        using var call =
            client.SubscribeTagNotifications(new SubscribeRequest(), cancellationToken: cancellationToken);

        while (!cancellationToken.IsCancellationRequested)
            try
            {
                await foreach (var notification in call.ResponseStream.ReadAllAsync(cancellationToken))
                    switch (notification.NotificationCase)
                    {
                        case TagNotification.NotificationOneofCase.TagCreated:
                        {
                            Dispatcher.UIThread.Post(() =>
                            {
                                messenger.Send(notification.TagCreated.ToNewEntityMessage());
                            });
                            break;
                        }
                        case TagNotification.NotificationOneofCase.TagUpdated:
                        {
                            Dispatcher.UIThread.Post(() =>
                            {
                                messenger.Send(notification.TagUpdated.ToClientNotification());
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