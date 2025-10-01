using System;
using System.Threading;
using System.Threading.Tasks;
using Client.Desktop.Communication.Notifications.Note.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.Lifecycle.Startup.Tasks.Streams;
using Client.Tracing.Tracing.Tracers;
using Global.Settings.UrlResolver;
using Grpc.Core;
using Grpc.Net.Client;
using Proto.Notifications.Note;

namespace Client.Desktop.Communication.Notifications.Note.Receiver;

public class NoteNotificationReceiver(
    IGrpcUrlBuilder grpcUrlBuilder,
    ITraceCollector tracer) : ILocalNoteNotificationPublisher, IStreamClient
{
    public event Func<ClientNoteUpdatedNotification, Task>? ClientNoteUpdatedNotificationReceived;
    public event Func<NewNoteMessage, Task>? NewNoteMessageReceived;

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

                var client = new NoteNotificationService.NoteNotificationServiceClient(channel);
                using var call =
                    client.SubscribeNoteNotifications(new SubscribeRequest(), cancellationToken: cancellationToken);

                attempt = 0;

                await foreach (var notification in call.ResponseStream.ReadAllAsync(cancellationToken))
                    await HandleNotificationReceived(notification, cancellationToken);

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

    private async Task HandleNotificationReceived(NoteNotification notification, CancellationToken stoppingToken)
    {
        switch (notification.NotificationCase)
        {
            case NoteNotification.NotificationOneofCase.NoteCreated:
            {
                var n = notification.NoteCreated;

                await tracer.Note.Create.NotificationReceived(GetType(),
                    Guid.Parse(n.NoteId), n);

                if (NewNoteMessageReceived == null)
                    throw new InvalidOperationException(
                        "Ticket data update received but no forwarding receiver is set");

                await NewNoteMessageReceived.Invoke(n.ToNewEntityMessage());

                break;
            }
            case NoteNotification.NotificationOneofCase.NoteUpdated:
            {
                var n = notification.NoteUpdated;

                await tracer.Note.Create.NotificationReceived(GetType(),
                    Guid.Parse(n.NoteId), n);

                if (ClientNoteUpdatedNotificationReceived == null)
                    throw new InvalidOperationException(
                        "Ticket data update received but no forwarding receiver is set");

                await ClientNoteUpdatedNotificationReceived.Invoke(n.ToClientNotification());

                break;
            }
            case NoteNotification.NotificationOneofCase.None:
                break;
        }
    }
}