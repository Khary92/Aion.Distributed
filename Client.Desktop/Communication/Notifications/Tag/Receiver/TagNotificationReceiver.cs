using System;
using System.Threading;
using System.Threading.Tasks;
using Client.Desktop.Communication.Notifications.Tag.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Lifecycle.Startup.Tasks.Streams;
using Client.Desktop.Services.Authentication;
using Client.Tracing.Tracing.Tracers;
using Global.Settings;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Proto.Notifications.Tag;

namespace Client.Desktop.Communication.Notifications.Tag.Receiver;

public class TagNotificationReceiver(
    IGrpcUrlService grpcUrlBuilder,
    ITraceCollector tracer,
    ITokenService tokenService) : ILocalTagNotificationPublisher, IStreamClient, IInitializeAsync
{
    private TagNotificationService.TagNotificationServiceClient? _client;

    public InitializationType Type => InitializationType.AuthToken;

    public async Task InitializeAsync()
    {
        var channel = GrpcChannel.ForAddress(grpcUrlBuilder.ClientToServerUrl);
        var tokenProvider = new Func<Task<string>>(tokenService.GetToken);
        var callInvoker = channel.Intercept(new AuthInterceptor(tokenProvider));
        _client = new TagNotificationService.TagNotificationServiceClient(callInvoker);

        await Task.CompletedTask;
    }

    public event Func<ClientTagUpdatedNotification, Task>? ClientTagUpdatedNotificationReceived;
    public event Func<NewTagMessage, Task>? NewTagMessageNotificationReceived;

    public async Task Publish(NewTagMessage message)
    {
        if (NewTagMessageNotificationReceived == null)
            throw new InvalidOperationException(
                "NewTagMessage received but no forwarding receiver is set");

        await NewTagMessageNotificationReceived.Invoke(message);
    }

    public async Task Publish(ClientTagUpdatedNotification notification)
    {
        if (ClientTagUpdatedNotificationReceived == null)
            throw new InvalidOperationException(
                "TagUpdatedNotification received but no forwarding receiver is set");

        await ClientTagUpdatedNotificationReceived.Invoke(notification);
    }


    public async Task StartListening(CancellationToken cancellationToken)
    {
        var attempt = 0;

        while (!cancellationToken.IsCancellationRequested)
            try
            {
                if (_client == null)
                    throw new InvalidOperationException("Client is not initialized");


                using var call =
                    _client.SubscribeTagNotifications(new SubscribeRequest(), cancellationToken: cancellationToken);

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

    private async Task HandleNotificationReceived(TagNotification notification)
    {
        switch (notification.NotificationCase)
        {
            case TagNotification.NotificationOneofCase.TagCreated:
            {
                var notificationTagCreated = notification.TagCreated;

                await tracer.Tag.Create.NotificationReceived(
                    GetType(),
                    Guid.Parse(notificationTagCreated.TraceData.TraceId),
                    notificationTagCreated);

                await Publish(notificationTagCreated.ToNewEntityMessage());

                break;
            }
            case TagNotification.NotificationOneofCase.TagUpdated:
            {
                var notificationTagUpdated = notification.TagUpdated;

                await tracer.Tag.Update.NotificationReceived(
                    GetType(),
                    Guid.Parse(notificationTagUpdated.TraceData.TraceId),
                    notificationTagUpdated);

                await Publish(notificationTagUpdated.ToClientNotification());

                break;
            }
            case TagNotification.NotificationOneofCase.None:
                break;
        }
    }
}