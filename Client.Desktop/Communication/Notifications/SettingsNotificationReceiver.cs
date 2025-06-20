using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client.Desktop.Communication.NotificationWrappers;
using Client.Desktop.DTO;
using CommunityToolkit.Mvvm.Messaging;
using Grpc.Core;
using Proto.Notifications.Settings;
using SubscribeRequest = Proto.Notifications.Settings.SubscribeRequest;

namespace Client.Desktop.Communication.Notifications;

public class SettingsNotificationReceiver(
    SettingsNotificationService.SettingsNotificationServiceClient client,
    IMessenger messenger)
{
    public async Task StartListeningAsync(CancellationToken cancellationToken)
    {
        using var call =
            client.SubscribeSettingsNotifications(new SubscribeRequest(), cancellationToken: cancellationToken);

        try
        {
            await foreach (var notification in call.ResponseStream.ReadAllAsync(cancellationToken))
                switch (notification.NotificationCase)
                {
                    case SettingsNotification.NotificationOneofCase.SettingsCreated:
                    {
                        var created = notification.SettingsCreated;
                        Dispatcher.UIThread.Post(() =>
                        {
                            messenger.Send(new NewSettingsMessage(new SettingsDto(
                                Guid.Parse(created.SettingsId),
                                created.ExportPath,
                                created.IsAddNewTicketsToCurrentSprintActive
                            )));
                        });
                        break;
                    }

                    case SettingsNotification.NotificationOneofCase.ExportPathChanged:
                    {
                        Dispatcher.UIThread.Post(() => { messenger.Send(notification.ExportPathChanged); });
                        break;
                    }

                    case SettingsNotification.NotificationOneofCase.AutomaticTicketAddingChanged:
                    {
                        Dispatcher.UIThread.Post(() => { messenger.Send(notification.AutomaticTicketAddingChanged); });
                        break;
                    }
                }
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in Settings notification listener: {ex}");
        }
    }
}