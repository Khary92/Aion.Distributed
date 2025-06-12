using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
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

        try
        {
            await foreach (var notification in call.ResponseStream.ReadAllAsync(cancellationToken))
            {
                switch (notification.NotificationCase)
                {
                    case UseCaseNotification.NotificationOneofCase.TimeSlotControlCreated:
                    {
                        Dispatcher.UIThread.Post(() => { messenger.Send(notification.TimeSlotControlCreated); });
                        break;
                    }
                    case UseCaseNotification.NotificationOneofCase.CreateSnapshot:
                    {
                        Dispatcher.UIThread.Post(() => { messenger.Send(notification.CreateSnapshot); });
                        break;
                    }
                    case UseCaseNotification.NotificationOneofCase.SaveDocumentation:
                    {
                        Dispatcher.UIThread.Post(() => { messenger.Send(notification.SaveDocumentation); });
                        break;
                    }
                    case UseCaseNotification.NotificationOneofCase.SprintSelectionChanged:
                    {
                        Dispatcher.UIThread.Post(() => { messenger.Send(notification.SprintSelectionChanged); });
                        break;
                    }
                    case UseCaseNotification.NotificationOneofCase.TraceReportSent:
                    {
                        Dispatcher.UIThread.Post(() => { messenger.Send(notification.TraceReportSent); });
                        break;
                    }
                    case UseCaseNotification.NotificationOneofCase.WorkDaySelectionChanged:
                    {
                        Dispatcher.UIThread.Post(() => { messenger.Send(notification.WorkDaySelectionChanged); });
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
            Console.WriteLine($"Error in UseCaseNotificationReceiver: {ex}");
        }
    }
}