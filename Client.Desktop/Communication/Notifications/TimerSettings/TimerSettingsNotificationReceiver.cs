using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client.Desktop.Lifecycle.Startup.Tasks.Streams;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using Grpc.Core;
using Proto.Notifications.TimerSettings;
using SubscribeRequest = Proto.Notifications.TimerSettings.SubscribeRequest;

namespace Client.Desktop.Communication.Notifications.TimerSettings;

public class TimerSettingsNotificationReceiver(
    TimerSettingsNotificationService.TimerSettingsNotificationServiceClient client,
    IMessenger messenger,
    ITraceCollector tracer) : IStreamClient
{
    public async Task StartListening(CancellationToken cancellationToken)
    {
        using var call =
            client.SubscribeTimerSettingsNotifications(new SubscribeRequest(), cancellationToken: cancellationToken);

        while (!cancellationToken.IsCancellationRequested)
            try
            {
                await foreach (var notification in call.ResponseStream.ReadAllAsync(cancellationToken))
                    switch (notification.NotificationCase)
                    {
                        case TimerSettingsNotification.NotificationOneofCase.DocuTimerSaveIntervalChanged:
                        {
                            var docuTimerSaveIntervalChangedNotification = notification.DocuTimerSaveIntervalChanged;

                            //TODO Add tracing

                            Dispatcher.UIThread.Post(() =>
                            {
                                messenger.Send(docuTimerSaveIntervalChangedNotification.ToClientNotification());
                            });
                            break;
                        }
                        case TimerSettingsNotification.NotificationOneofCase.SnapshotSaveIntervalChanged:
                        {
                            var snapshotSaveIntervalChangedNotification = notification.SnapshotSaveIntervalChanged;

                            //TODO Add tracing

                            Dispatcher.UIThread.Post(() =>
                            {
                                messenger.Send(snapshotSaveIntervalChangedNotification.ToClientNotification());
                            });
                            break;
                        }
                    }
            }
            catch (Exception ex)
            {
                Console.WriteLine(GetType() + " caused an exception: " + ex);

                if (cancellationToken.IsCancellationRequested)
                    return;

                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            }
    }
}