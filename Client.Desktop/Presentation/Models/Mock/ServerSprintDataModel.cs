using System;
using System.Collections.Concurrent;
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
using Client.Proto;
using Google.Protobuf.WellKnownTypes;
using Proto.Command.Sprints;
using Proto.DTO.Sprint;
using Proto.Requests.Sprints;
using ReactiveUI;
using Service.Proto.Shared.Commands.Sprints;
using Service.Proto.Shared.Requests.Sprints;

namespace Client.Desktop.Presentation.Models.Mock;

public class ServerSprintDataModel(MockDataService mockDataService) : ReactiveObject, IInitializeAsync, ISprintCommandSender,
    ISprintRequestSender, ILocalSprintNotificationPublisher, IStreamClient
{
    private ObservableCollection<SprintClientModel> _sprints = [];

    public ObservableCollection<SprintClientModel> Sprints
    {
        get => _sprints;
        set => this.RaiseAndSetIfChanged(ref _sprints, value);
    }
    
    public InitializationType Type => InitializationType.MockServices;

    public Task InitializeAsync()
    {
        Sprints.Clear();
        Sprints = new ObservableCollection<SprintClientModel>(mockDataService.Sprints);
        return Task.CompletedTask;
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

    public Task<SprintProto?> Send(GetActiveSprintRequestProto request)
    {
        var activeSprint = Sprints.FirstOrDefault(s => s.IsActive);

        if (activeSprint == null)
        {
            return Task.FromResult<SprintProto?>(null);
        }

        var result = new SprintProto()
        {
            SprintId = activeSprint.SprintId.ToString(),
            Name = activeSprint.Name,
            Start = Timestamp.FromDateTimeOffset(activeSprint.StartTime),
            End = Timestamp.FromDateTimeOffset(activeSprint.EndTime),
            IsActive = activeSprint.IsActive,
            TicketIds = { activeSprint.TicketIds.ToRepeatedField() }
        };

        return Task.FromResult(result)!;
    }

    public Task<SprintListProto> Send(GetAllSprintsRequestProto request)
    {
        var list = new SprintListProto();

        foreach (var sprint in Sprints)
        {
            list.Sprints.Add(new SprintProto()
            {
                SprintId = sprint.SprintId.ToString(),
                Name = sprint.Name,
                Start = Timestamp.FromDateTimeOffset(sprint.StartTime),
                End = Timestamp.FromDateTimeOffset(sprint.EndTime),
                IsActive = sprint.IsActive,
                TicketIds = { sprint.TicketIds.ToRepeatedField() }
            });
        }

        return Task.FromResult(list);
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

    private readonly TimeSpan _responseDelay = TimeSpan.FromMilliseconds(50);
    private readonly ConcurrentQueue<CreateSprintCommandProto> _createQueue = new();
    private readonly ConcurrentQueue<UpdateSprintDataCommandProto> _updateDataQueue = new();
    private readonly ConcurrentQueue<AddTicketToActiveSprintCommandProto> _addTicketToSprintQueue = new();
    private readonly ConcurrentQueue<SetSprintActiveStatusCommandProto> _sendActiveStatusQueue = new();

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
                await Task.Delay(_responseDelay, cancellationToken);
                await Publish(notification);
            }

            while (_sendActiveStatusQueue.TryDequeue(out var activeStatusCmd))
            {
                var notification = new ClientSprintActiveStatusSetNotification();
                await Task.Delay(_responseDelay, cancellationToken);
                await Publish(notification);
            }

            if (_sendActiveStatusQueue.IsEmpty && _addTicketToSprintQueue.IsEmpty && _createQueue.IsEmpty &&
                _updateDataQueue.IsEmpty)
                await Task.Delay(50, cancellationToken);
        }
    }
}