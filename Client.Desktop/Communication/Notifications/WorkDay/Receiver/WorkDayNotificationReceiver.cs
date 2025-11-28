using System;
using System.Threading;
using System.Threading.Tasks;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Lifecycle.Startup.Tasks.Streams;
using Client.Desktop.Services.Authentication;
using Client.Tracing.Tracing.Tracers;
using Global.Settings;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Proto.Notifications.WorkDay;

namespace Client.Desktop.Communication.Notifications.WorkDay.Receiver;

public class WorkDayNotificationReceiver(
    IGrpcUrlService grpcUrlBuilder,
    ITraceCollector tracer,
    ITokenService tokenService) : ILocalWorkDayNotificationPublisher, IStreamClient, IInitializeAsync
{
    private WorkDayNotificationService.WorkDayNotificationServiceClient? _client;

    public InitializationType Type => InitializationType.AuthToken;

    public async Task InitializeAsync()
    {
        var channel = GrpcChannel.ForAddress(grpcUrlBuilder.ClientToServerUrl);
        var tokenProvider = new Func<Task<string>>(tokenService.GetToken);
        var callInvoker = channel.Intercept(new AuthInterceptor(tokenProvider));
        _client = new WorkDayNotificationService.WorkDayNotificationServiceClient(callInvoker);

        await Task.CompletedTask;
    }

    public event Func<NewWorkDayMessage, Task>? NewWorkDayMessageReceived;

    public async Task Publish(NewWorkDayMessage message)
    {
        if (NewWorkDayMessageReceived == null)
            throw new InvalidOperationException(
                "NewWorkDayMessage received but no forwarding receiver is set");

        await NewWorkDayMessageReceived.Invoke(message);
    }

    public async Task StartListening(CancellationToken cancellationToken)
    {
        var attempt = 0;

        while (!cancellationToken.IsCancellationRequested)
            try
            {
                if (_client is null)
                    throw new InvalidOperationException("Client is not initialized");

                using var call =
                    _client.SubscribeWorkDayNotifications(new SubscribeRequest(), cancellationToken: cancellationToken);

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

    private async Task HandleNotificationReceived(WorkDayNotification notification)
    {
        switch (notification.NotificationCase)
        {
            case WorkDayNotification.NotificationOneofCase.WorkDayCreated:
            {
                var notificationWorkDayCreated = notification.WorkDayCreated;

                await tracer.WorkDay.Create.NotificationReceived(
                    GetType(),
                    Guid.Parse(notificationWorkDayCreated.TraceData.TraceId),
                    notificationWorkDayCreated);

                await Publish(notificationWorkDayCreated.ToNewEntityMessage());

                break;
            }
            case WorkDayNotification.NotificationOneofCase.None:
                break;
        }
    }
}