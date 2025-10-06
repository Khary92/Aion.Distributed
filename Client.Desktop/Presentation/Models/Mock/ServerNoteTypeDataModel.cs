using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client.Desktop.Communication.Mock;
using Client.Desktop.Communication.Notifications.NoteType.Receiver;
using Client.Desktop.Communication.Notifications.NoteType.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.DataModels;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Lifecycle.Startup.Tasks.Streams;
using Proto.Command.NoteTypes;
using Proto.DTO.NoteType;
using Proto.Requests.NoteTypes;
using ReactiveUI;
using Service.Proto.Shared.Commands.NoteTypes;
using Service.Proto.Shared.Requests.NoteTypes;

namespace Client.Desktop.Presentation.Models.Mock;

public class ServerNoteTypeDataModel(MockDataService mockDataService) : ReactiveObject, IInitializeAsync,
    INoteTypeCommandSender, INoteTypeRequestSender, ILocalNoteTypeNotificationPublisher, IStreamClient
{
    private ObservableCollection<NoteTypeClientModel> _noteTypes = [];

    public ObservableCollection<NoteTypeClientModel> NoteTypes
    {
        get => _noteTypes;
        set => this.RaiseAndSetIfChanged(ref _noteTypes, value);
    }

    public InitializationType Type => InitializationType.MockServices;

    public Task InitializeAsync()
    {
        NoteTypes.Clear();
        NoteTypes = new ObservableCollection<NoteTypeClientModel>(mockDataService.NoteTypes);
        return Task.CompletedTask;
    }

    private readonly TimeSpan _responseDelay = TimeSpan.FromMilliseconds(50);
    private readonly ConcurrentQueue<CreateNoteTypeCommandProto> _createQueue = new();
    private readonly ConcurrentQueue<ChangeNoteTypeNameCommandProto> _updateNameQueue = new();
    private readonly ConcurrentQueue<ChangeNoteTypeColorCommandProto> _updateColorQueue = new();

    public Task<bool> Send(CreateNoteTypeCommandProto command)
    {
        _createQueue.Enqueue(command);
        return Task.FromResult(true);
    }

    public Task<bool> Send(ChangeNoteTypeNameCommandProto command)
    {
        _updateNameQueue.Enqueue(command);
        return Task.FromResult(true);
    }

    public Task<bool> Send(ChangeNoteTypeColorCommandProto command)
    {
        _updateColorQueue.Enqueue(command);
        return Task.FromResult(true);
    }

    public Task<GetAllNoteTypesResponseProto> Send(GetAllNoteTypesRequestProto request)
    {
        var list = new GetAllNoteTypesResponseProto();

        foreach (var noteType in NoteTypes)
        {
            list.NoteTypes.Add(new NoteTypeProto()
            {
                NoteTypeId = noteType.NoteTypeId.ToString(),
                Name = noteType.Name,
                Color = noteType.Color
            });
        }

        return Task.FromResult(list);
    }


    public Task<NoteTypeProto> Send(GetNoteTypeByIdRequestProto request)
    {
        var noteType = NoteTypes.FirstOrDefault(nt => nt.NoteTypeId == Guid.Parse(request.NoteTypeId));

        if (noteType == null) throw new Exception("Note type not found");

        var result = new NoteTypeProto()
        {
            NoteTypeId = noteType.NoteTypeId.ToString(),
            Name = noteType.Name,
            Color = noteType.Color
        };

        return Task.FromResult(result);
    }

    public event Func<ClientNoteTypeColorChangedNotification, Task>? ClientNoteTypeColorChangedNotificationReceived;
    public event Func<ClientNoteTypeNameChangedNotification, Task>? ClientNoteTypeNameChangedNotificationReceived;
    public event Func<NewNoteTypeMessage, Task>? NewNoteTypeMessageReceived;

    public Task Publish(NewNoteTypeMessage message)
    {
        if (NewNoteTypeMessageReceived is { } handler)
            return handler(message);
        return Task.CompletedTask;
    }

    public Task Publish(ClientNoteTypeColorChangedNotification notification)
    {
        if (ClientNoteTypeColorChangedNotificationReceived is { } handler)
            return handler(notification);
        return Task.CompletedTask;
    }

    public Task Publish(ClientNoteTypeNameChangedNotification notification)
    {
        if (ClientNoteTypeNameChangedNotificationReceived is { } handler)
            return handler(notification);
        return Task.CompletedTask;
    }

    public async Task StartListening(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            while (_updateNameQueue.TryDequeue(out var changeNoteTypeNameCommand))
            {
                var notification = new ClientNoteTypeNameChangedNotification(
                    Guid.Parse(changeNoteTypeNameCommand.NoteTypeId),
                    changeNoteTypeNameCommand.Name,
                    Guid.Parse(changeNoteTypeNameCommand.TraceData.TraceId)
                );

                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    var ticket = NoteTypes.FirstOrDefault(nt =>
                        nt.NoteTypeId == Guid.Parse(changeNoteTypeNameCommand.NoteTypeId));
                    ticket?.Apply(notification);
                });

                await Task.Delay(_responseDelay, cancellationToken);
                await Publish(notification);
            }

            while (_createQueue.TryDequeue(out var createCmd))
            {
                var notification =
                    new NewNoteTypeMessage(
                        new NoteTypeClientModel(Guid.Parse(createCmd.NoteTypeId), createCmd.Name, createCmd.Color),
                        Guid.Parse(createCmd.TraceData.TraceId));

                await Dispatcher.UIThread.InvokeAsync(() => { NoteTypes.Add(notification.NoteType); });

                await Task.Delay(_responseDelay, cancellationToken);
                await Publish(notification);
            }

            while (_updateColorQueue.TryDequeue(out var changeNoteTypeColorCommand))
            {
                var notification = new ClientNoteTypeColorChangedNotification(
                    Guid.Parse(changeNoteTypeColorCommand.NoteTypeId), changeNoteTypeColorCommand.Color,
                    Guid.Parse(changeNoteTypeColorCommand.TraceData.TraceId));

                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    var ticket = NoteTypes.FirstOrDefault(nt =>
                        nt.NoteTypeId == Guid.Parse(changeNoteTypeColorCommand.NoteTypeId));
                    ticket?.Apply(notification);
                });

                await Task.Delay(_responseDelay, cancellationToken);
                await Publish(notification);
            }

            if (_updateNameQueue.IsEmpty && _updateColorQueue.IsEmpty && _createQueue.IsEmpty)
                await Task.Delay(50, cancellationToken);
        }
    }
}