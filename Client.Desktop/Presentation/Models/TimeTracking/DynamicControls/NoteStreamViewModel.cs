using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.Commands.Notes.Records;
using Client.Desktop.Communication.Local;
using Client.Desktop.Communication.Notifications.Note.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.Requests.Notes.Records;
using Client.Desktop.DataModels;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Lifecycle.Startup.Tasks.Register;
using Client.Desktop.Presentation.Factories;
using Client.Desktop.Services.LocalSettings;
using Client.Tracing.Tracing.Tracers;
using ReactiveUI;

namespace Client.Desktop.Presentation.Models.TimeTracking.DynamicControls;

public class NoteStreamViewModel(
    ICommandSender commandSender,
    IRequestSender requestSender,
    INotificationPublisherFacade notificationPublisher,
    INoteViewFactory noteViewFactory,
    ILocalSettingsService localSettingsService,
    ITraceCollector tracer)
    : ReactiveObject, IMessengerRegistration, IInitializeAsync
{
    private Guid _ticketId;
    private Guid _timeSlotId;
    public ObservableCollection<NoteViewModel> Notes { get; } = [];

    public Guid TicketId
    {
        get => _ticketId;
        set => this.RaiseAndSetIfChanged(ref _ticketId, value);
    }

    public Guid TimeSlotId
    {
        get => _timeSlotId;
        set => this.RaiseAndSetIfChanged(ref _timeSlotId, value);
    }

    public InitializationType Type => InitializationType.ViewModel;

    public async Task InitializeAsync()
    {
        Notes.Clear();
        
        var noteClientModels =
            await requestSender.Send(new ClientGetNotesByTimeSlotIdRequest(TimeSlotId, Guid.NewGuid()));
        
        foreach (var note in noteClientModels) await InsertNoteViewModel(note);
    }

    public void RegisterMessenger()
    {
        notificationPublisher.Note.NewNoteMessageReceived += HandleNewNoteMessage;
        notificationPublisher.Note.ClientNoteUpdatedNotificationReceived += HandleClientNoteUpdatedNotification;
    }

    public void UnregisterMessenger()
    {
        notificationPublisher.Note.NewNoteMessageReceived -= HandleNewNoteMessage;
        notificationPublisher.Note.ClientNoteUpdatedNotificationReceived -= HandleClientNoteUpdatedNotification;
    }

    public async Task AddNoteControl()
    {
        if (!localSettingsService.IsSelectedDateCurrentDate()) return;

        var tracingId = Guid.NewGuid();

        await tracer.Note.Create.StartUseCase(GetType(), tracingId);

        var clientCreateNoteCommand = new ClientCreateNoteCommand(Guid.NewGuid(), Guid.Empty, string.Empty, TicketId,
            TimeSlotId, DateTimeOffset.Now, tracingId);

        await tracer.Note.Create.SendingCommand(GetType(), tracingId, clientCreateNoteCommand);
        await commandSender.Send(clientCreateNoteCommand);
    }

    private async Task HandleNewNoteMessage(NewNoteMessage message)
    {
        await InsertNoteViewModel(message.Note);
        await tracer.Note.Create.AggregateAdded(GetType(), message.TraceId);
    }

    private Task HandleClientNoteUpdatedNotification(ClientNoteUpdatedNotification notification)
    {
        var viewModel = Notes.FirstOrDefault(n => n.Note.NoteId == notification.NoteId);

        if (viewModel == null) return Task.CompletedTask;

        viewModel.Note.Apply(notification);
        return Task.CompletedTask;
    }

    private async Task InsertNoteViewModel(NoteClientModel noteClientModel)
    {
        var noteViewModel = await noteViewFactory.Create(noteClientModel);

        Notes.Insert(0, noteViewModel);
    }
}