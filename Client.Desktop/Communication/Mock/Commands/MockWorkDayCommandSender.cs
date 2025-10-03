using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands.WorkDays;
using Client.Desktop.Communication.Commands.WorkDays.Records;
using Client.Desktop.Communication.Notifications.WorkDay.Receiver;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.DataModels;
using Client.Desktop.Lifecycle.Startup.Tasks.Streams;

namespace Client.Desktop.Communication.Mock.Commands;

public class MockWorkDayCommandSender : IWorkDayCommandSender, ILocalWorkDayNotificationPublisher, IStreamClient
{
    private readonly TimeSpan _responseDelay = TimeSpan.FromMilliseconds(50);
    private readonly ConcurrentQueue<ClientCreateWorkDayCommand> _workDayQueue = new();

    public event Func<NewWorkDayMessage, Task>? NewWorkDayMessageReceived;

    public async Task Publish(NewWorkDayMessage message)
    {
        if (NewWorkDayMessageReceived is { } handler)
            await handler(message);
    }

    public async Task StartListening(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            while (_workDayQueue.TryDequeue(out var command))
            {
                var workDay = new WorkDayClientModel(command.WorkDayId, command.Date);
                var message = new NewWorkDayMessage(workDay, command.TraceId);

                await Task.Delay(_responseDelay, cancellationToken);
                await Publish(message);
            }

            if (_workDayQueue.IsEmpty)
                await Task.Delay(50, cancellationToken);
        }
    }

    public Task<bool> Send(ClientCreateWorkDayCommand command)
    {
        _workDayQueue.Enqueue(command);
        return Task.FromResult(true);
    }
}