using System;
using System.Threading;
using System.Threading.Tasks;
using Client.Avalonia.Communication.NotificationProcessors.Messages;
using CommunityToolkit.Mvvm.Messaging;
using Contract.DTO;
using Grpc.Core;
using Proto.Notifications.TimerSettings;

namespace Client.Avalonia.Communication.Notifications.TimerSettings;

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
                        messenger.Send(new NewTimerSettingsMessage(new TimerSettingsDto(
                            Guid.Parse(created.TimerSettingsId),
                            created.DocumentationSaveInterval,
                            created.SnapshotSaveInterval
                        )));
                        break;
                    }
                    case TimerSettingsNotification.NotificationOneofCase.DocuTimerSaveIntervalChanged:
                    {
                        messenger.Send(notification.DocuTimerSaveIntervalChanged);
                        break;
                    }
                    case TimerSettingsNotification.NotificationOneofCase.SnapshotSaveIntervalChanged:
                    {
                        messenger.Send(notification.SnapshotSaveIntervalChanged);
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