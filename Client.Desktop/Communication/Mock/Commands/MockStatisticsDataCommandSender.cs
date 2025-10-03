using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands.StatisticsData;
using Client.Desktop.Communication.Commands.StatisticsData.Records;
using Client.Desktop.Communication.Notifications.StatisticsData.Receiver;
using Client.Desktop.Communication.Notifications.StatisticsData.Records;
using Client.Desktop.Lifecycle.Startup.Tasks.Streams;

namespace Client.Desktop.Communication.Mock.Commands;

public class MockStatisticsDataCommandSender : IStatisticsDataCommandSender, ILocalStatisticsDataNotificationPublisher,
    IStreamClient
{
    private readonly ConcurrentQueue<ClientChangeTagSelectionCommand> _tagSelectionQueue = new();
    private readonly ConcurrentQueue<ClientChangeProductivityCommand> _productivityQueue = new();

    private readonly TimeSpan _responseDelay = TimeSpan.FromMilliseconds(50);

    public Task<bool> Send(ClientChangeTagSelectionCommand command)
    {
        _tagSelectionQueue.Enqueue(command);
        return Task.FromResult(true);
    }

    public Task<bool> Send(ClientChangeProductivityCommand command)
    {
        _productivityQueue.Enqueue(command);
        return Task.FromResult(true);
    }

    public event Func<ClientChangeProductivityNotification, Task>? ClientChangeProductivityNotificationReceived;
    public event Func<ClientChangeTagSelectionNotification, Task>? ClientChangeTagSelectionNotificationReceived;

    public async Task Publish(ClientChangeProductivityNotification notification)
    {
        if (ClientChangeProductivityNotificationReceived is { } handler)
            await handler(notification);
    }

    public async Task Publish(ClientChangeTagSelectionNotification notification)
    {
        if (ClientChangeTagSelectionNotificationReceived is { } handler)
            await handler(notification);
    }

    public async Task StartListening(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            while (_tagSelectionQueue.TryDequeue(out var tagCommand))
            {
                var notification = new ClientChangeTagSelectionNotification(tagCommand.StatisticsDataId,
                    tagCommand.SelectedTagIds, tagCommand.TraceId);
                await Task.Delay(_responseDelay, cancellationToken);
                await Publish(notification);
            }

            while (_productivityQueue.TryDequeue(out var prodCommand))
            {
                var notification = new ClientChangeProductivityNotification(prodCommand.StatisticsDataId,
                    prodCommand.IsProductive, prodCommand.IsNeutral, prodCommand.IsUnproductive, prodCommand.TraceId);
                await Task.Delay(_responseDelay, cancellationToken);
                await Publish(notification);
            }

            if (_tagSelectionQueue.IsEmpty && _productivityQueue.IsEmpty)
            {
                await Task.Delay(50, cancellationToken);
            }
        }
    }
}