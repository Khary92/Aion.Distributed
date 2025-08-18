using Grpc.Core;
using Grpc.Net.Client;
using Proto.Notifications.NoteType;
using Service.Admin.Tracing;
using Service.Admin.Web.Communication.NoteType.State;

namespace Service.Admin.Web.Communication.NoteType;

public class NoteTypeNotificationReceiver(INoteTypeStateService noteTypeStateService, ITraceCollector tracer)
{
    public async Task SubscribeToNotifications(CancellationToken stoppingToken = default)
    {
        var channelOptions = new GrpcChannelOptions
        {
            HttpHandler = new SocketsHttpHandler
            {
                EnableMultipleHttp2Connections = true,
                KeepAlivePingDelay = TimeSpan.FromSeconds(60),
                KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
                PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan
            }
        };

        var channel = GrpcChannel.ForAddress("http://core-service:8080", channelOptions);
        var client = new NoteTypeProtoNotificationService.NoteTypeProtoNotificationServiceClient(channel);

        while (!stoppingToken.IsCancellationRequested)
            try
            {
                using var call =
                    client.SubscribeNoteNotifications(new SubscribeRequest(), cancellationToken: stoppingToken);

                await foreach (var notification in call.ResponseStream.ReadAllAsync(stoppingToken))
                    switch (notification.NotificationCase)
                    {
                        case NoteTypeNotification.NotificationOneofCase.NoteTypeCreated:
                        {
                            var newNoteTypeMessage = notification.NoteTypeCreated.ToWebModel();

                            await tracer.NoteType.Create.NotificationReceived(GetType(), newNoteTypeMessage.TraceId,
                                notification);

                            await noteTypeStateService.AddNoteType(newNoteTypeMessage);
                            break;
                        }
                        case NoteTypeNotification.NotificationOneofCase.NoteTypeColorChanged:
                        {
                            var webNoteTypeColorChangedNotification =
                                notification.NoteTypeColorChanged.ToNotification();

                            await tracer.NoteType.ChangeColor.NotificationReceived(GetType(),
                                webNoteTypeColorChangedNotification.TraceId, notification);

                            await noteTypeStateService.Apply(webNoteTypeColorChangedNotification);
                            break;
                        }
                        case NoteTypeNotification.NotificationOneofCase.NoteTypeNameChanged:
                        {
                            var webNoteTypeNameChangedNotification = notification.NoteTypeNameChanged.ToNotification();

                            await tracer.NoteType.ChangeName.NotificationReceived(GetType(),
                                webNoteTypeNameChangedNotification.TraceId, notification);

                            await noteTypeStateService.Apply(webNoteTypeNameChangedNotification);
                            break;
                        }
                    }
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Unavailable)
            {
                await Task.Delay(5000, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                await Task.Delay(5000, stoppingToken);
            }
    }
}