using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client.Desktop.Lifecycle.Startup.Tasks.Streams;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using Global.Settings.Types;
using Global.Settings.UrlResolver;
using Grpc.Core;
using Grpc.Net.Client;
using Proto.Notifications.Client;

namespace Client.Desktop.Communication.Notifications.Client.Receiver;

public class ClientNotificationReceiver(
    IGrpcUrlBuilder grpcUrlBuilder,
    IMessenger messenger,
    ITraceCollector tracer) : IStreamClient
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

                var client = new ClientNotificationService.ClientNotificationServiceClient(channel);
                using var call =
                    client.SubscribeClientNotifications(new SubscribeRequest(), cancellationToken: cancellationToken);

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

    private async Task HandleNotificationReceived(ClientNotification notification)
    {
        switch (notification.NotificationCase)
        {
            case ClientNotification.NotificationOneofCase.TimeSlotControlCreated:
            {
                await tracer.Client.CreateTrackingControl.NotificationReceived(GetType(),
                    Guid.Parse(notification.TimeSlotControlCreated.TraceData.TraceId),
                    notification.TimeSlotControlCreated);

                Dispatcher.UIThread.Post(() =>
                {
                    messenger.Send(notification.TimeSlotControlCreated.ToClientNotification());
                });
                break;
            }
            case ClientNotification.NotificationOneofCase.SprintSelectionChanged:
            {
                Dispatcher.UIThread.Post(() =>
                {
                    messenger.Send(notification.SprintSelectionChanged.ToClientNotification());
                });
                break;
            }
        }
    }
}