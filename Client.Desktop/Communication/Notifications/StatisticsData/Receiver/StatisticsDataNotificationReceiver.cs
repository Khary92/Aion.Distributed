using System;
using System.Threading;
using System.Threading.Tasks;
using Client.Desktop.Communication.Notifications.StatisticsData.Records;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Lifecycle.Startup.Tasks.Streams;
using Client.Desktop.Services.Authentication;
using Client.Tracing.Tracing.Tracers;
using Global.Settings;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Proto.Notifications.StatisticsData;
using SubscribeRequest = Proto.Notifications.StatisticsData.SubscribeRequest;

namespace Client.Desktop.Communication.Notifications.StatisticsData.Receiver;

public class StatisticsDataNotificationReceiver(
    IGrpcUrlService grpcUrlBuilder,
    ITraceCollector tracer,
    ITokenService tokenService) : ILocalStatisticsDataNotificationPublisher, IStreamClient, IInitializeAsync
{
    private StatisticsDataNotificationService.StatisticsDataNotificationServiceClient? _client;

    public InitializationType Type => InitializationType.AuthToken;

    public async Task InitializeAsync()
    {
        var channel = GrpcChannel.ForAddress(grpcUrlBuilder.ClientToServerUrl);
        var tokenProvider = new Func<Task<string>>(tokenService.GetToken);
        var callInvoker = channel.Intercept(new AuthInterceptor(tokenProvider));
        _client = new StatisticsDataNotificationService.StatisticsDataNotificationServiceClient(callInvoker);

        await Task.CompletedTask;
    }

    public event Func<ClientChangeProductivityNotification, Task>? ClientChangeProductivityNotificationReceived;
    public event Func<ClientChangeTagSelectionNotification, Task>? ClientChangeTagSelectionNotificationReceived;

    public async Task Publish(ClientChangeProductivityNotification notification)
    {
        if (ClientChangeProductivityNotificationReceived == null)
            throw new InvalidOperationException(
                "ChangeProductivityNotification received but no forwarding receiver is set");

        await ClientChangeProductivityNotificationReceived.Invoke(notification);
    }

    public async Task Publish(ClientChangeTagSelectionNotification notification)
    {
        if (ClientChangeTagSelectionNotificationReceived == null)
            throw new InvalidOperationException(
                "ChangeTagSelectionNotification received but no forwarding receiver is set");

        await ClientChangeTagSelectionNotificationReceived.Invoke(notification);
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
                    _client.SubscribeStatisticsDataNotifications(new SubscribeRequest(),
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

    private async Task HandleNotificationReceived(StatisticsDataNotification notification)
    {
        switch (notification.NotificationCase)
        {
            case StatisticsDataNotification.NotificationOneofCase.ChangeProductivity:
            {
                var notificationChangeProductivity = notification.ChangeProductivity;
                await tracer.Statistics.ChangeProductivity.NotificationReceived(
                    GetType(),
                    Guid.Parse(notificationChangeProductivity.TraceData.TraceId),
                    notificationChangeProductivity);

                await Publish(notificationChangeProductivity.ToClientNotification());

                break;
            }
            case StatisticsDataNotification.NotificationOneofCase.ChangeTagSelection:
            {
                var notificationChangeTagSelection = notification.ChangeTagSelection;
                await tracer.Statistics.ChangeTagSelection.NotificationReceived(
                    GetType(),
                    Guid.Parse(notificationChangeTagSelection.TraceData.TraceId),
                    notificationChangeTagSelection);

                await Publish(notificationChangeTagSelection.ToClientNotification());

                break;
            }
            case StatisticsDataNotification.NotificationOneofCase.None:
                break;
        }
    }
}