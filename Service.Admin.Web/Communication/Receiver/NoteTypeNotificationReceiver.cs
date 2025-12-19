using Global.Settings;
using Grpc.Core;
using Grpc.Net.Client;
using Proto.Notifications.NoteType;
using Service.Admin.Tracing;
using Service.Admin.Web.Communication.Authentication;
using Service.Admin.Web.Communication.Mapper;
using Service.Admin.Web.Services.State;

namespace Service.Admin.Web.Communication.Receiver;

public class NoteTypeNotificationReceiver
{
    private readonly INoteTypeStateService _noteTypeStateService;
    private readonly ITraceCollector _tracer;
    private readonly JwtService _jwtService;
    private readonly NoteTypeProtoNotificationService.NoteTypeProtoNotificationServiceClient _client;

    public NoteTypeNotificationReceiver(
        INoteTypeStateService noteTypeStateService,
        ITraceCollector tracer,
        IGrpcUrlService grpcUrlBuilder,
        JwtService jwtService)
    {
        _noteTypeStateService = noteTypeStateService;
        _tracer = tracer;
        _jwtService = jwtService;
        var channel = GrpcChannel.ForAddress(grpcUrlBuilder.InternalToServerUrl,
            new GrpcChannelOptions
            {
                HttpHandler = new SocketsHttpHandler
                {
                    EnableMultipleHttp2Connections = true,
                    KeepAlivePingDelay = TimeSpan.FromSeconds(60),
                    KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
                    PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan
                },
                Credentials = ChannelCredentials.Insecure,
                UnsafeUseInsecureChannelCallCredentials = true
            });

        _client = new NoteTypeProtoNotificationService.NoteTypeProtoNotificationServiceClient(channel);
    }
    
    public async Task SubscribeToNotifications(CancellationToken stoppingToken = default)
    {
        while (!stoppingToken.IsCancellationRequested)
            try
            {
                using var call = _client.SubscribeNoteNotifications(
                    new SubscribeRequest(),
                    headers: new Metadata { { "Authorization", $"Bearer {_jwtService.Token}" } },
                    cancellationToken: stoppingToken);

                await foreach (var notification in call.ResponseStream.ReadAllAsync(stoppingToken))
                    switch (notification.NotificationCase)
                    {
                        case NoteTypeNotification.NotificationOneofCase.NoteTypeCreated:
                        {
                            var newNoteTypeMessage = notification.NoteTypeCreated.ToWebModel();

                            await _tracer.NoteType.Create.NotificationReceived(GetType(), newNoteTypeMessage.TraceId,
                                notification);

                            await _noteTypeStateService.AddNoteType(newNoteTypeMessage);
                            break;
                        }
                        case NoteTypeNotification.NotificationOneofCase.NoteTypeColorChanged:
                        {
                            var webNoteTypeColorChangedNotification =
                                notification.NoteTypeColorChanged.ToNotification();

                            await _tracer.NoteType.ChangeColor.NotificationReceived(GetType(),
                                webNoteTypeColorChangedNotification.TraceId, notification);

                            await _noteTypeStateService.Apply(webNoteTypeColorChangedNotification);
                            break;
                        }
                        case NoteTypeNotification.NotificationOneofCase.NoteTypeNameChanged:
                        {
                            var webNoteTypeNameChangedNotification = notification.NoteTypeNameChanged.ToNotification();

                            await _tracer.NoteType.ChangeName.NotificationReceived(GetType(),
                                webNoteTypeNameChangedNotification.TraceId, notification);

                            await _noteTypeStateService.Apply(webNoteTypeNameChangedNotification);
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