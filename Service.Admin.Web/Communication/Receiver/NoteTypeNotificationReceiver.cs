using Global.Settings.UrlResolver;
using Grpc.Core;
using Grpc.Net.Client;
using Proto.Notifications.NoteType;
using Service.Admin.Tracing;
using Service.Admin.Web.Communication.Mapper;
using Service.Admin.Web.Services.State;

namespace Service.Admin.Web.Communication.Receiver;

public class NoteTypeNotificationReceiver(
    INoteTypeStateService noteTypeStateService,
    ITraceCollector tracer,
    IGrpcUrlBuilder grpcUrlBuilder)
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

        var channel =
            GrpcChannel.ForAddress(
                grpcUrlBuilder
                    .From(ResolvingServices.WebAdmin)
                    .To(ResolvingServices.Server)
                    .BuildAddress(),
                channelOptions);

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
                        case NoteTypeNotification.NotificationOneofCase.None:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
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
                Console.WriteLine(GetType() + " caused an error: " + ex);
                await Task.Delay(5000, stoppingToken);
            }
    }
}