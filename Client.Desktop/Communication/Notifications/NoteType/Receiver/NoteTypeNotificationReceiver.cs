using System;
using System.Threading;
using System.Threading.Tasks;
using Client.Desktop.Communication.Notifications.NoteType.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.Lifecycle.Startup.Tasks.Streams;
using Client.Tracing.Tracing.Tracers;
using Global.Settings.UrlResolver;
using Grpc.Core;
using Grpc.Net.Client;
using Proto.Notifications.NoteType;

namespace Client.Desktop.Communication.Notifications.NoteType.Receiver;

public class NoteTypeNotificationReceiver(
    IGrpcUrlBuilder grpcUrlBuilder,
    ITraceCollector tracer) : ILocalNoteTypeNotificationPublisher, IStreamClient
{
    public event Func<ClientNoteTypeColorChangedNotification, Task>? ClientNoteTypeColorChangedNotificationReceived;
    public event Func<ClientNoteTypeNameChangedNotification, Task>? ClientNoteTypeNameChangedNotificationReceived;
    public event Func<NewNoteTypeMessage, Task>? NewNoteTypeMessageReceived;

    public async Task Publish(NewNoteTypeMessage message)
    {
        if (NewNoteTypeMessageReceived == null)
            throw new InvalidOperationException(
                "NewNoteTypeMessage received but no forwarding receiver is set");

        await NewNoteTypeMessageReceived.Invoke(message);
    }

    public async Task Publish(ClientNoteTypeColorChangedNotification notification)
    {
        if (ClientNoteTypeColorChangedNotificationReceived == null)
            throw new InvalidOperationException(
                "NoteTypeColorChangedNotification received but no forwarding receiver is set");

        await ClientNoteTypeColorChangedNotificationReceived.Invoke(notification);
    }

    public async Task Publish(ClientNoteTypeNameChangedNotification notification)
    {
        if (ClientNoteTypeNameChangedNotificationReceived == null)
            throw new InvalidOperationException(
                "NoteTypeNameChangedNotification received but no forwarding receiver is set");

        await ClientNoteTypeNameChangedNotificationReceived.Invoke(notification);
    }


    public async Task StartListening(CancellationToken cancellationToken)
    {
        var attempt = 0;

        while (!cancellationToken.IsCancellationRequested)
            try
            {
                using var channel = GrpcChannel.ForAddress(grpcUrlBuilder
                    .From(ResolvingServices.Client)
                    .To(ResolvingServices.Server)
                    .BuildAddress());

                var client = new NoteTypeProtoNotificationService.NoteTypeProtoNotificationServiceClient(channel);
                using var call =
                    client.SubscribeNoteNotifications(new SubscribeRequest(), cancellationToken: cancellationToken);

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

    private async Task HandleNotificationReceived(NoteTypeNotification notification)
    {
        switch (notification.NotificationCase)
        {
            case NoteTypeNotification.NotificationOneofCase.NoteTypeColorChanged:
            {
                var n = notification.NoteTypeColorChanged;

                await tracer.NoteType.ChangeColor.NotificationReceived(
                    GetType(),
                    Guid.Parse(n.TraceData.TraceId),
                    n);

                await Publish(n.ToClientNotification());

                break;
            }
            case NoteTypeNotification.NotificationOneofCase.NoteTypeCreated:
            {
                var n = notification.NoteTypeCreated;

                await tracer.NoteType.Create.NotificationReceived(
                    GetType(),
                    Guid.Parse(n.TraceData.TraceId),
                    n);

                await Publish(n.ToNewEntityMessage());

                break;
            }
            case NoteTypeNotification.NotificationOneofCase.NoteTypeNameChanged:
            {
                var n = notification.NoteTypeNameChanged;

                await tracer.NoteType.ChangeName.NotificationReceived(
                    GetType(),
                    Guid.Parse(n.TraceData.TraceId),
                    n);

                await Publish(n.ToClientNotification());

                break;
            }
            case NoteTypeNotification.NotificationOneofCase.None:
                break;
        }
    }
}