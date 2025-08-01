using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client.Desktop.Lifecycle.Startup.Tasks.Streams;
using CommunityToolkit.Mvvm.Messaging;
using Grpc.Core;
using Proto.Notifications.UseCase;

namespace Client.Desktop.Communication.Notifications.UseCase;

public class UseCaseNotificationReceiver(
    UseCaseNotificationService.UseCaseNotificationServiceClient client,
    IMessenger messenger) : IStreamClient
{
    public async Task StartListening(CancellationToken cancellationToken)
    {
        using var call =
            client.SubscribeUseCaseNotifications(new SubscribeRequest(), cancellationToken: cancellationToken);

        while (!cancellationToken.IsCancellationRequested)
            try
            {
                await foreach (var notification in call.ResponseStream.ReadAllAsync(cancellationToken))
                    switch (notification.NotificationCase)
                    {
                        case UseCaseNotification.NotificationOneofCase.TimeSlotControlCreated:
                        {
                            Dispatcher.UIThread.Post(() =>
                            {
                                messenger.Send(notification.TimeSlotControlCreated.ToClientNotification());
                            });
                            break;
                        }
                        case UseCaseNotification.NotificationOneofCase.CreateSnapshot:
                        {
                            Dispatcher.UIThread.Post(() =>
                            {
                                messenger.Send(notification.CreateSnapshot.ToClientNotification());
                            });
                            break;
                        }
                        case UseCaseNotification.NotificationOneofCase.SaveDocumentation:
                        {
                            Dispatcher.UIThread.Post(() =>
                            {
                                messenger.Send(notification.SaveDocumentation.ToClientNotification());
                            });
                            break;
                        }
                        case UseCaseNotification.NotificationOneofCase.SprintSelectionChanged:
                        {
                            Dispatcher.UIThread.Post(() =>
                            {
                                messenger.Send(notification.SprintSelectionChanged.ToClientNotification());
                            });
                            break;
                        }
                        case UseCaseNotification.NotificationOneofCase.WorkDaySelectionChanged:
                        {
                            Dispatcher.UIThread.Post(() =>
                            {
                                messenger.Send(notification.WorkDaySelectionChanged.ToClientNotification());
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