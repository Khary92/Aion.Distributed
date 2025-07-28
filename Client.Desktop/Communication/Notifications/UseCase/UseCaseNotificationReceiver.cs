using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client.Desktop.Communication.Notifications.UseCase.Records;
using CommunityToolkit.Mvvm.Messaging;
using Grpc.Core;
using Proto.Notifications.UseCase;

namespace Client.Desktop.Communication.Notifications.UseCase;

public class UseCaseNotificationReceiver(
    UseCaseNotificationService.UseCaseNotificationServiceClient client,
    IMessenger messenger)
{
    public async Task StartListeningAsync(CancellationToken cancellationToken)
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
                            Dispatcher.UIThread.Post(() => { messenger.Send(new ClientCreateSnapshotNotification()); });
                            break;
                        }
                        case UseCaseNotification.NotificationOneofCase.SaveDocumentation:
                        {
                            Dispatcher.UIThread.Post(() =>
                            {
                                messenger.Send(new ClientSaveDocumentationNotification());
                            });
                            break;
                        }
                        case UseCaseNotification.NotificationOneofCase.SprintSelectionChanged:
                        {
                            Dispatcher.UIThread.Post(() =>
                            {
                                messenger.Send(new ClientSprintSelectionChangedNotification());
                            });
                            break;
                        }
                        case UseCaseNotification.NotificationOneofCase.WorkDaySelectionChanged:
                        {
                            Dispatcher.UIThread.Post(() =>
                            {
                                messenger.Send(new ClientWorkDaySelectionChangedNotification());
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