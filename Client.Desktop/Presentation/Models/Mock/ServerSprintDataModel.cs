using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client.Desktop.Communication.Mock;
using Client.Desktop.Communication.Notifications.Sprint.Receiver;
using Client.Desktop.Communication.Notifications.Sprint.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.DataModels;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Lifecycle.Startup.Tasks.Streams;
using Proto.Command.Sprints;
using Proto.Requests.Sprints;
using ReactiveUI;
using Service.Proto.Shared.Commands.Sprints;
using ISprintRequestSender = Client.Desktop.Communication.Requests.Sprint.ISprintRequestSender;

namespace Client.Desktop.Presentation.Models.Mock;

public class ServerSprintDataModel(MockDataService mockDataService) : ReactiveObject, IInitializeAsync,
    ISprintCommandSender,
    ISprintRequestSender, ILocalSprintNotificationPublisher, IStreamClient
{
    private readonly ConcurrentQueue<AddTicketToActiveSprintCommandProto> _addTicketToSprintQueue = new();
    private readonly ConcurrentQueue<CreateSprintCommandProto> _createQueue = new();

    private readonly TimeSpan _responseDelay = TimeSpan.FromMilliseconds(50);
    private readonly ConcurrentQueue<SetSprintActiveStatusCommandProto> _sendActiveStatusQueue = new();
    private readonly ConcurrentQueue<UpdateSprintDataCommandProto> _updateDataQueue = new();
    private ObservableCollection<SprintClientModel> _sprints = [];

    public ObservableCollection<SprintClientModel> Sprints
    {
        get => _sprints;
        set => this.RaiseAndSetIfChanged(ref _sprints, value);
    }

    public InitializationType Type => InitializationType.MockModels;

    public Task InitializeAsync()
    {
        Sprints.Clear();
        Sprints = new ObservableCollection<SprintClientModel>(mockDataService.Sprints);
        return Task.CompletedTask;
    }

    public event Func<ClientSprintActiveStatusSetNotification, Task>? ClientSprintActiveStatusSetNotificationReceived;
    public event Func<ClientSprintDataUpdatedNotification, Task>? ClientSprintDataUpdatedNotificationReceived;

    public event Func<ClientTicketAddedToActiveSprintNotification, Task>?
        ClientTicketAddedToActiveSprintNotificationReceived;

    public event Func<NewSprintMessage, Task>? NewSprintMessageReceived;

    public async Task Publish(NewSprintMessage message)
    {
        if (NewSprintMessageReceived is { } handler)
            await handler(message);
    }

    public async Task Publish(ClientSprintActiveStatusSetNotification notification)
    {
        if (ClientSprintActiveStatusSetNotificationReceived is { } handler)
            await handler(notification);
    }

    public async Task Publish(ClientSprintDataUpdatedNotification notification)
    {
        if (ClientSprintDataUpdatedNotificationReceived is { } handler)
            await handler(notification);
    }

    public async Task Publish(ClientTicketAddedToActiveSprintNotification notification)
    {
        if (ClientTicketAddedToActiveSprintNotificationReceived is { } handler)
            await handler(notification);
    }

    public Task<bool> Send(CreateSprintCommandProto command)
    {
        _createQueue.Enqueue(command);
        return Task.FromResult(true);
    }

    public Task<bool> Send(AddTicketToActiveSprintCommandProto command)
    {
        _addTicketToSprintQueue.Enqueue(command);
        return Task.FromResult(true);
    }

    public Task<bool> Send(SetSprintActiveStatusCommandProto command)
    {
        _sendActiveStatusQueue.Enqueue(command);
        return Task.FromResult(true);
    }

    public Task<bool> Send(UpdateSprintDataCommandProto command)
    {
        _updateDataQueue.Enqueue(command);
        return Task.FromResult(true);
    }

    public Task<SprintClientModel?> Send(GetActiveSprintRequestProto request)
    {
        var activeSprint = Sprints.FirstOrDefault(s => s.IsActive);

        if (activeSprint == null) return Task.FromResult<SprintClientModel?>(null);

        var result = new SprintClientModel(activeSprint.SprintId, activeSprint.Name, activeSprint.IsActive,
            activeSprint.StartTime, activeSprint.EndTime, activeSprint.TicketIds);

        return Task.FromResult(result)!;
    }

    public Task<List<SprintClientModel>> Send(GetAllSprintsRequestProto request)
    {
        var list = new List<SprintClientModel>();

        foreach (var sprint in Sprints)
            list.Add(new SprintClientModel(sprint.SprintId, sprint.Name, sprint.IsActive,
                sprint.StartTime, sprint.EndTime, sprint.TicketIds));

        return Task.FromResult(list);
    }

    public async Task StartListening(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            while (_createQueue.TryDequeue(out var createCmd))
            {
                var notification =
                    new NewSprintMessage(new SprintClientModel(Guid.Parse(createCmd.SprintId), createCmd.Name,
                        createCmd.IsActive, createCmd.StartTime.ToDateTimeOffset(),
                        createCmd.EndTime.ToDateTimeOffset(), []), Guid.NewGuid());

                mockDataService.Sprints.Add(notification.Sprint);

                await Dispatcher.UIThread.InvokeAsync(() => { Sprints.Add(notification.Sprint); });

                await Task.Delay(_responseDelay, cancellationToken);
                await Publish(notification);
            }

            while (_updateDataQueue.TryDequeue(out var updateCmd))
            {
                var notification = new ClientSprintDataUpdatedNotification(Guid.Parse(updateCmd.SprintId),
                    updateCmd.Name,
                    updateCmd.StartTime.ToDateTimeOffset(), updateCmd.EndTime.ToDateTimeOffset(),
                    Guid.Parse(updateCmd.TraceData.TraceId)
                );

                var mockDataSprint =
                    mockDataService.Sprints.FirstOrDefault(s => s.SprintId == Guid.Parse(updateCmd.SprintId));
                mockDataSprint?.Apply(notification);

                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    var sprint = Sprints.FirstOrDefault(s => s.SprintId == Guid.Parse(updateCmd.SprintId));
                    sprint?.Apply(notification);
                });

                await Task.Delay(_responseDelay, cancellationToken);
                await Publish(notification);
            }

            while (_addTicketToSprintQueue.TryDequeue(out var activeSprintCmd))
            {
                var notification = new ClientTicketAddedToActiveSprintNotification();

                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    foreach (var sprint in Sprints)
                    {
                        if (!sprint.IsActive) continue;

                        var mockedDataSprint = mockDataService.Sprints.First(s => s.SprintId == sprint.SprintId);
                        var mockedDataTicket =
                            mockDataService.Tickets.First(t => t.TicketId == Guid.Parse(activeSprintCmd.TicketId));

                        mockedDataSprint.TicketIds.Add(mockedDataTicket.TicketId);
                        mockedDataTicket.SprintIds.Add(mockedDataSprint.SprintId);

                        var ticket =
                            mockDataService.Tickets.First(t => t.TicketId == Guid.Parse(activeSprintCmd.TicketId));
                        ticket.SprintIds.Add(sprint.SprintId);
                        sprint.TicketIds.Add(ticket.TicketId);
                    }
                });

                await Task.Delay(_responseDelay, cancellationToken);
                await Publish(notification);
            }

            while (_sendActiveStatusQueue.TryDequeue(out var activeStatusCmd))
            {
                var notification = new ClientSprintActiveStatusSetNotification();

                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    foreach (var sprint in Sprints)
                    {
                        var mockedDataSprint =
                            mockDataService.Sprints.FirstOrDefault(s => s.SprintId == sprint.SprintId);

                        if (sprint.SprintId != Guid.Parse(activeStatusCmd.SprintId))
                        {
                            if (mockedDataSprint != null) mockedDataSprint.IsActive = false;
                            sprint.IsActive = false;
                            continue;
                        }

                        if (mockedDataSprint != null) mockedDataSprint.IsActive = activeStatusCmd.IsActive;
                        sprint.IsActive = activeStatusCmd.IsActive;
                    }
                });

                await Task.Delay(_responseDelay, cancellationToken);
                await Publish(notification);
            }

            if (_sendActiveStatusQueue.IsEmpty && _addTicketToSprintQueue.IsEmpty && _createQueue.IsEmpty &&
                _updateDataQueue.IsEmpty)
                await Task.Delay(50, cancellationToken);
        }
    }
}