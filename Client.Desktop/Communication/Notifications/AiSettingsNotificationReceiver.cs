using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client.Desktop.Communication.NotificationWrappers;
using CommunityToolkit.Mvvm.Messaging;
using Contract.DTO;
using Grpc.Core;
using Proto.Notifications.AiSettings;

namespace Client.Desktop.Communication.Notifications;

public class AiSettingsNotificationReceiver(
    AiSettingsNotificationService.AiSettingsNotificationServiceClient client,
    IMessenger messenger)
{
    public async Task StartListeningAsync(CancellationToken cancellationToken)
    {
        using var call =
            client.SubscribeAiSettingsNotifications(new SubscribeRequest(), cancellationToken: cancellationToken);

        try
        {
            await foreach (var notification in call.ResponseStream.ReadAllAsync(cancellationToken))
            {
                switch (notification.NotificationCase)
                {
                    case AiSettingsNotification.NotificationOneofCase.AiSettingsCreated:
                    {
                        var created = notification.AiSettingsCreated;
                        Dispatcher.UIThread.Post(() => { messenger.Send(new NewAiSettingsMessage(new AiSettingsDto(
                            Guid.Parse(created.AiSettingsId),
                            created.Prompt,
                            created.LanguageModelPath
                        ))); });
                        break;
                    }

                    case AiSettingsNotification.NotificationOneofCase.LanguageModelChanged:
                    {
                        Dispatcher.UIThread.Post(() =>
                        {
                            messenger.Send(notification.LanguageModelChanged);
                        });
                        break;
                    }

                    case AiSettingsNotification.NotificationOneofCase.PromptChanged:
                    {
                        Dispatcher.UIThread.Post(() =>
                        {
                            messenger.Send(notification.PromptChanged);
                        });
                        break;
                    }
                }
            }
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in AiSettings notification listener: {ex}");
        }
    }
}