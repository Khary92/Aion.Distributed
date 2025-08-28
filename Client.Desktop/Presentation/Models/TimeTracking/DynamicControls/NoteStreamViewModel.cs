using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.Commands.Notes.Records;
using Client.Desktop.Communication.Notifications.Note.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.Requests.Notes.Records;
using Client.Desktop.DataModels;
using Client.Desktop.DataModels.Local;
using Client.Desktop.Presentation.Factories;
using Client.Desktop.Services.LocalSettings;
using Client.Desktop.Services.LocalSettings.Commands;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using ReactiveUI;

namespace Client.Desktop.Presentation.Models.TimeTracking.DynamicControls;

public class NoteStreamViewModel(
    ICommandSender commandSender,
    IRequestSender requestSender,
    IMessenger messenger,
    INoteViewFactory noteViewFactory,
    ILocalSettingsService localSettingsService,
    ITraceCollector tracer)
    : ReactiveObject
{
    private Guid _ticketId;
    private Guid _timeSlotId;
    private SettingsClientModel? _settingsClient;
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

    public void RegisterMessenger()
    {
        messenger.Register<SettingsClientModel>(this, async void (_, m) =>
        {
            _settingsClient = m;
            await InitializeAsync();
        });

        messenger.Register<WorkDaySelectedNotification>(this, async void (_, m) =>
        {
            _settingsClient!.SelectedDate = m.Date;
            await InitializeAsync();
        });

        messenger.Register<NewNoteMessage>(this,
            async void (_, message) => { await InsertNoteViewModel(message.Note); });

        messenger.Register<ClientNoteUpdatedNotification>(this, (_, notification) =>
        {
            var viewModel = Notes.FirstOrDefault(n => n.Note.NoteId == notification.NoteId);

            if (viewModel == null) return;

            viewModel.Note.Apply(notification);
        });
    }

    public async Task InitializeAsync()
    {
        var noteDtos = await requestSender.Send(new ClientGetNotesByTimeSlotIdRequest(TimeSlotId, Guid.NewGuid()));
        foreach (var note in noteDtos) await InsertNoteViewModel(note);
    }

    private async Task InsertNoteViewModel(NoteClientModel noteClientModel)
    {
        var noteViewModel = await noteViewFactory.Create(new NoteClientModel(noteClientModel.NoteId,
            noteClientModel.Text, noteClientModel.NoteTypeId,
            noteClientModel.TimeSlotId, noteClientModel.TimeStamp));

        Notes.Insert(0, noteViewModel);
    }
}