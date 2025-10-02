using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.Commands.Notes.Records;
using Client.Desktop.Communication.Local;
using Client.Desktop.Communication.Notifications.Note.Records;
using Client.Desktop.Communication.Notifications.NoteType.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.Requests.NoteType;
using Client.Desktop.DataModels;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Lifecycle.Startup.Tasks.Register;
using Client.Tracing.Tracing.Tracers;
using DynamicData;
using ReactiveUI;
using Unit = System.Reactive.Unit;

namespace Client.Desktop.Presentation.Models.TimeTracking.DynamicControls;

public class NoteViewModel : ReactiveObject, IMessengerRegistration, IInitializeAsync
{
    private readonly ICommandSender _commandSender;
    private readonly INotificationPublisherFacade _notificationPublisher;
    private readonly IRequestSender _requestSender;
    private readonly ITraceCollector _tracer;
    private int _currentNoteTypeIndex;

    public NoteViewModel(ICommandSender commandSender, IRequestSender requestSender,
        ITraceCollector tracer, INotificationPublisherFacade notificationPublisher)
    {
        _commandSender = commandSender;
        _requestSender = requestSender;
        _tracer = tracer;
        _notificationPublisher = notificationPublisher;

        SetNextTypeCommand = ReactiveCommand.CreateFromTask(SetNextType);
        SetPreviousTypeCommand = ReactiveCommand.CreateFromTask(SetPreviousType);
        UpdateNoteCommand = ReactiveCommand.CreateFromTask(Update);
    }

    public NoteClientModel Note { get; set; } = null!;

    private ObservableCollection<NoteTypeClientModel> NoteTypes { get; } = [];
    public ReactiveCommand<Unit, Unit> SetNextTypeCommand { get; }
    public ReactiveCommand<Unit, Unit> SetPreviousTypeCommand { get; }
    public ReactiveCommand<Unit, Unit> UpdateNoteCommand { get; }

    public InitializationType Type => InitializationType.ViewModel;

    public async Task InitializeAsync()
    {
        NoteTypes.Clear();
        
        var noteTypeViewModels = await _requestSender.Send(new ClientGetAllNoteTypesRequest());
        
        NoteTypes.AddRange(noteTypeViewModels);

        if (Note.NoteTypeId == Guid.Empty || !NoteTypes.Any()) return;

        Note.NoteType = NoteTypes.FirstOrDefault(nt => nt.NoteTypeId == Note.NoteTypeId);
    }

    public void RegisterMessenger()
    {
        _notificationPublisher.Note.ClientNoteUpdatedNotificationReceived += HandleClientNoteUpdatedNotification;
        _notificationPublisher.NoteType.NewNoteTypeMessageReceived += HandleNewNoteTypeMessage;
        _notificationPublisher.NoteType.ClientNoteTypeColorChangedNotificationReceived +=
            HandleClientNoteTypeColorChangedNotification;
    }

    public void UnregisterMessenger()
    {
        _notificationPublisher.Note.ClientNoteUpdatedNotificationReceived -= HandleClientNoteUpdatedNotification;
        _notificationPublisher.NoteType.NewNoteTypeMessageReceived -= HandleNewNoteTypeMessage;
        _notificationPublisher.NoteType.ClientNoteTypeColorChangedNotificationReceived -=
            HandleClientNoteTypeColorChangedNotification;
    }

    private async Task HandleClientNoteTypeNameChangedNotification(ClientNoteTypeNameChangedNotification notification)
    {
        var noteType = NoteTypes.FirstOrDefault(nt => nt.NoteTypeId == notification.NoteTypeId);

        if (noteType == null) return;

        noteType.Apply(notification);

        await _tracer.NoteType.ChangeName.ChangesApplied(GetType(), notification.TraceId);
    }

    private async Task HandleClientNoteTypeColorChangedNotification(ClientNoteTypeColorChangedNotification notification)
    {
        var noteType = NoteTypes.FirstOrDefault(nt => nt.NoteTypeId == notification.NoteTypeId);

        if (noteType == null) return;

        noteType.Apply(notification);


        await _tracer.NoteType.ChangeColor.ChangesApplied(GetType(), notification.TraceId);
    }

    private async Task HandleNewNoteTypeMessage(NewNoteTypeMessage message)
    {
        NoteTypes.Add(message.NoteType);
        await _tracer.NoteType.Create.AggregateAdded(GetType(), message.TraceId);
    }

    private async Task HandleClientNoteUpdatedNotification(ClientNoteUpdatedNotification notification)
    {
        if (Note.NoteId != notification.NoteId) return;

        Note.Apply(notification);
        Note.NoteType = NoteTypes.FirstOrDefault(nt => nt.NoteTypeId == Note.NoteTypeId);

        await _tracer.Note.Update.ChangesApplied(GetType(), notification.TraceId);
    }

    private async Task Update()
    {
        var traceId = Guid.NewGuid();
        await _tracer.Note.Update.StartUseCase(GetType(), traceId);

        var clientUpdateNoteCommand = new ClientUpdateNoteCommand
        (
            Note.NoteId,
            Note.Text,
            Note.NoteType?.NoteTypeId ?? Guid.Empty, Note.TimeSlotId,
            traceId
        );

        await _tracer.Note.Update.SendingCommand(GetType(), traceId, clientUpdateNoteCommand);
        await _commandSender.Send(clientUpdateNoteCommand);
    }

    private Task SetNextType()
    {
        if (NoteTypes.Count == 0 || _currentNoteTypeIndex >= NoteTypes.Count - 1) return Task.CompletedTask;

        _currentNoteTypeIndex++;

        Note.NoteType = NoteTypes[_currentNoteTypeIndex];
        return Task.CompletedTask;
    }

    private Task SetPreviousType()
    {
        if (NoteTypes.Count == 0 || _currentNoteTypeIndex == 0) return Task.CompletedTask;

        _currentNoteTypeIndex--;

        Note.NoteType = NoteTypes[_currentNoteTypeIndex];
        return Task.CompletedTask;
    }
}