using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client.Desktop.Communication.Notifications.TimerSettings.Records;
using CommunityToolkit.Mvvm.Messaging;
using Global.Settings.UrlResolver;
using Grpc.Core;
using Grpc.Net.Client;
using Proto.Notifications.TimerSettings;
using SubscribeRequest = Proto.Notifications.TimerSettings.SubscribeRequest;

namespace Client.Desktop.Communication.Notifications.TimerSettings.Receiver;

public class TimerSettingsNotificationReceiver(
    IGrpcUrlBuilder grpcUrlBuilder,
    IMessenger messenger) : ILocalTimerSettingsNotificationPublisher
{
    public async Task StartListening(CancellationToken cancellationToken)
    {
        var attempt = 0;

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                using var channel = GrpcChannel.ForAddress(grpcUrlBuilder
                    .From(ResolvingServices.Client)
                    .To(ResolvingServices.Server)
                    .BuildAddress());

                var client = new TimerSettingsNotificationService.TimerSettingsNotificationServiceClient(channel);
                using var call =
                    client.SubscribeTimerSettingsNotifications(new SubscribeRequest(),
                        cancellationToken: cancellationToken);

                attempt = 0;

                await foreach (var notification in call.ResponseStream.ReadAllAsync(cancellationToken))
                    await HandleNotificationReceived(notification);

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled &&
                                          cancellationToken.IsCancellationRequested)
            {
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{GetType()} caused an exception: {ex}");

                if (cancellationToken.IsCancellationRequested)
                    return;

                attempt++;
                var backoffSeconds = Math.Min(30, Math.Pow(2, attempt));
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(backoffSeconds), cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    return;
                }
            }
        }
    }

    private async Task HandleNotificationReceived(TimerSettingsNotification notification)
    {
        switch (notification.NotificationCase)
        {
            case TimerSettingsNotification.NotificationOneofCase.DocuTimerSaveIntervalChanged:
            {
                var n = notification.DocuTimerSaveIntervalChanged;

                // TODO: Add tracing

                if (ClientDocuTimerSaveIntervalChangedNotificationReceived == null)
                {
                    throw new InvalidOperationException(
                        "Ticket data update received but no forwarding receiver is set");
                }

                await ClientDocuTimerSaveIntervalChangedNotificationReceived.Invoke(n.ToClientNotification());
                
                break;
            }
            case TimerSettingsNotification.NotificationOneofCase.SnapshotSaveIntervalChanged:
            {
                var n = notification.SnapshotSaveIntervalChanged;

                // TODO: Add tracing
                if (ClientSnapshotSaveIntervalChangedNotificationReceived == null)
                {
                    throw new InvalidOperationException(
                        "Ticket data update received but no forwarding receiver is set");
                }

                await ClientSnapshotSaveIntervalChangedNotificationReceived.Invoke(n.ToClientNotification());
                
                break;
            }
            case TimerSettingsNotification.NotificationOneofCase.None:
                break;
        }
    }

    public event Func<ClientDocuTimerSaveIntervalChangedNotification, Task>? ClientDocuTimerSaveIntervalChangedNotificationReceived;
    public event Func<ClientSnapshotSaveIntervalChangedNotification, Task>? ClientSnapshotSaveIntervalChangedNotificationReceived;
}