using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client.Desktop.Communication.NotificationWrappers;
using Client.Desktop.DTO;
using CommunityToolkit.Mvvm.Messaging;
using Grpc.Core;
using Proto.Notifications.TimerSettings;

namespace Client.Desktop.Communication.Notifications;

public class TimerSettingsNotificationReceiver(
    TimerSettingsNotificationService.TimerSettingsNotificationServiceClient client,
    IMessenger messenger)
{
    public async Task StartListeningAsync(CancellationToken cancellationToken)
    {
        using var call =
            client.SubscribeTimerSettingsNotifications(new SubscribeRequest(), cancellationToken: cancellationToken);

        try
        {
            await foreach (var notification in call.ResponseStream.ReadAllAsync(cancellationToken))
            {
                switch (notification.NotificationCase)
                {
                    case TimerSettingsNotification.NotificationOneofCase.TimerSettingsCreated:
                    {
                        var created = notification.TimerSettingsCreated;

                        Dispatcher.UIThread.Post(() =>
                        {
                            messenger.Send(new NewTimerSettingsMessage(new TimerSettingsDto(
                                Guid.Parse(created.TimerSettingsId),
                                created.DocumentationSaveInterval,
                                created.SnapshotSaveInterval
                            )));
                        });
                        break;
                    }
                    case TimerSettingsNotification.NotificationOneofCase.DocuTimerSaveIntervalChanged:
                    {
                        Dispatcher.UIThread.Post(() => { messenger.Send(notification.DocuTimerSaveIntervalChanged); });
                        break;
                    }
                    case TimerSettingsNotification.NotificationOneofCase.SnapshotSaveIntervalChanged:
                    {
                        Dispatcher.UIThread.Post(() => { messenger.Send(notification.SnapshotSaveIntervalChanged); });
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
            Console.WriteLine($"Error in TimerSettingsNotificationReceiver: {ex}");
        }
    }
}